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
    public class EggGroupService : BaseService, IEggGroupService
    {
        public EggGroupService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache, cached)
        {
        }

        public async Task<EggGroupDTO?> AddEggGroup(PostEggGroupDTO model)
        {
            if (await _uow.EggGroups.ExistsByNameAsync(model.egName))
                throw new BadRequestException($"Egg Group with name {model.egName} Exists");

            var newEggGroup = _mapper.Map<EggGroup>(model);
            await _uow.EggGroups.AddAsync(newEggGroup);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"EggGroup:all:");
            }
            else
            {
                throw new BadRequestException("EggGroup Post Fail");
            }

            return _mapper.Map<EggGroupDTO?>(newEggGroup);
        }

        public async Task<int> ImportEggGroupAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var egDtos = csv.GetRecords<PostEggGroupDTO>()
            .Where(x => !string.IsNullOrWhiteSpace(x.egName))
            .GroupBy(x => x.egName.Trim().ToLower())
            .Select(g => g.First())
            .ToList();

            var names = egDtos.Select(x => x.egName.Trim()).ToList();

            var existingNames = await _uow.EggGroups.GetExistingNamesAsync(names);

            var newDtos = egDtos.Where(x => !existingNames.Contains(x.egName.Trim())).ToList();

            var eggGroups = _mapper.Map<List<EggGroup>>(newDtos);

            if (!eggGroups.Any())
                return 0;

            await _uow.EggGroups.AddRangeAsync(eggGroups);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"EggGroup:all:");
            }
            else
            {
                throw new BadRequestException("Something wrong with eggroup list");
            }

            return eggGroups.Count;
        }

        public async Task<bool> DeleteEggGroup(Guid egID)
        {
            var eggGroup = await _uow.EggGroups.GetByIdAsync(egID);

            if (eggGroup == null)
                throw new NotFoundException($"Egg Group with id {egID} not found");

            await _uow.EggGroups.RemoveAsync(eggGroup);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"EggGroup:all:");
            }
            return saved;
        }

        public async Task<PagedResult<EggGroupDTO>> GetAllEggGroup(QueryParams queryParams)
        { 
            var cacheKey = $"EggGroup:all:page:{queryParams.PageNumber}:size:{queryParams.PageSize}";
            try
            {
                var cached = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cached))
                {
                    return JsonSerializer.Deserialize<PagedResult<EggGroupDTO>>(cached)!;
                }
            }
            catch(Exception ex)
            {
                Log.Error($"cache Save Fail {ex}");
            }

            var result = await GetPagedAsync<EggGroup, EggGroupDTO>(
                queryParams,
                _uow.EggGroups.GetAllAsync
            );

            try
            {
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
                    });
            }
            catch(Exception ex)
            {
                Log.Error($"cache write Fail {ex}");
            }
            return result;
        }

        public async Task<EggGroupDTO> GetEggGroupById(Guid egID)
        {
            var eggGroup = await _uow.EggGroups.GetByIdAsync(egID);
            if (eggGroup == null)
                throw new NotFoundException($"Egg Group with id {egID} not found");

            return _mapper.Map<EggGroupDTO>(eggGroup);
        }

        public async Task<bool> UpdateEggGroup(Guid egID, PutEggGroupDTO model)
        {
            var eggGroup = await _uow.EggGroups.GetByIdAsync(egID);

            if (eggGroup == null)
                throw new NotFoundException($"Egg Group with id {egID} not found");

            _mapper.Map(model, eggGroup);

            await _uow.EggGroups.UpdateAsync(eggGroup);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"EggGroup:all:");
            }

            return saved;

        }
    }
}
