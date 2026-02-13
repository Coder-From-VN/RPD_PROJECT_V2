using AutoMapper;
using CsvHelper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using Serilog;
using System.Globalization;
using System.Text.Json;

namespace RPD_API.Service
{
    public class AbilitiesService : BaseService, IAbilitiesService
    {

        public AbilitiesService(
        IUnitOfWorkRepo uow,
        IMapper mapper,
        IDistributedCache cache,
         ICacheService cached)
        : base(uow, mapper, cache,cached)
        {
        }

        public async Task<AbilitiesDTO?> PostAbilities(PostAbilitiesDTO model)
        {
            if (await _uow.Abilities.ExistsByNameAsync(model.abName))
                throw new BadRequestException("Ability name already exists");

            var newAbilities = _mapper.Map<Abilities>(model);

            await _uow.Abilities.AddAsync(newAbilities);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"abilities:all:");
            }
            else
            {
                throw new BadRequestException("Ability Post Fail");
            }

            return _mapper.Map<AbilitiesDTO?>(newAbilities);

        }

        public async Task<int> ImportAbilitiesAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("Abilities File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var abilityDtos = csv.GetRecords<PostAbilitiesDTO>()
                                    .Where(x => !string.IsNullOrWhiteSpace(x.abName))
                                    .GroupBy(x => x.abName.Trim().ToLower())
                                    .Select(g => g.First())
                                    .ToList();

            var names = abilityDtos
                .Select(x => x.abName.Trim())
                .ToList();

            var existingNames = await _uow.Abilities
                .GetExistingNamesAsync(names);

            var newDtos = abilityDtos
                .Where(x => !existingNames.Contains(x.abName))
                .ToList();

            var abilities = _mapper.Map<List<Abilities>>(newDtos);

            if (!abilities.Any())
                return 0;

            await _uow.Abilities.AddRangeAsync(abilities);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"abilities:all:");
            }
            else
            {
                throw new BadRequestException("something worng with abilities list");
            }

            return abilities.Count;
        }

        public async Task<AbilitiesDTO?> GetAbilitiesById(Guid abID)
        {
            var ability = await _uow.Abilities.GetByIdAsync(abID);
            if (ability == null)
                throw new NotFoundException($"Ability with id {abID} not found");

            return _mapper.Map<AbilitiesDTO>(ability);
        }

        public async Task<PagedResult<AbilitiesDTO>> GetAllAbilities(QueryParams queryParams)
        {
            var cacheKey = $"abilities:all:page:{queryParams.PageNumber}:size:{queryParams.PageSize}";
            try
            {
                var cached = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cached))
                {
                    return JsonSerializer.Deserialize<PagedResult<AbilitiesDTO>>(cached)!;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"cache read Fail {ex}");
            }
            

            var result = await GetPagedAsync<Abilities, AbilitiesDTO>(
                queryParams,
                _uow.Abilities.GetAllAsync
            );

            try
            {
                await _cache.SetStringAsync(cacheKey,JsonSerializer.Serialize(result), 
                    new DistributedCacheEntryOptions {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
                    });
            }
            catch(Exception ex)
            {
                Log.Error($"Cache write Fail {ex}");
            }
            return result;
        }

        public async Task<bool> PutAbilities(Guid abID, PutAbilitiesDTO model)
        {
            var abilities = await _uow.Abilities.GetByIdAsync(abID);
            if (abilities == null)
                throw new NotFoundException("Ability not found");

            _mapper.Map(model, abilities);

            await _uow.Abilities.UpdateAsync(abilities);
            
            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"abilities:all:");
            }

            return saved;
        }

        public async Task<bool> DeleteAbilities(Guid guid)
        {
            var deleteThisAbilities = await _uow.Abilities.GetByIdAsync(guid);
            if (deleteThisAbilities == null)
                throw new NotFoundException("Ability not found");

            await _uow.Abilities.RemoveAsync(deleteThisAbilities);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"abilities:all:");
            }

            return saved;
        }


    }
}
