using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.Globalization;
using System.Text.Json;

namespace RPD_API.Service
{
    public class EggGroupService : BaseService, IEggGroupService
    {
        public EggGroupService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<EggGroupDTO?> AddEggGroup(PostEggGroupDTO model)
        {
            if (await _uow.EggGroups.ExistsByNameAsync(model.egName))
                throw new BadRequestException($"Egg Group with name {model.egName} Exists");

            var newEggGroup = _mapper.Map<EggGroup>(model);
            await _uow.EggGroups.AddAsync(newEggGroup);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<EggGroupDTO?>(newEggGroup) : null;
        }

        public async Task<int> ImportEggGroupAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var egDtos = csv.GetRecords<PostEggGroupDTO>().ToList();

            var normalizedDtos = egDtos
                .Where(x => !string.IsNullOrWhiteSpace(x.egName))
                .Select(x => new PostEggGroupDTO
                {
                    egName = x.egName.Trim(),
                })
                .GroupBy(x => x.egName.ToLower())
                .Select(g => g.First())
                .ToList();

            var names = normalizedDtos
                .Select(x => x.egName)
                .ToList();

            var existingNames = await _uow.EggGroups
                .GetExistingNamesAsync(names);

            var newDtos = normalizedDtos
                .Where(x => !existingNames.Contains(x.egName))
                .ToList();

            var eggGroups = _mapper.Map<List<EggGroup>>(newDtos);

            if (!eggGroups.Any())
                return 0;

            await _uow.EggGroups.AddRangeAsync(eggGroups);

            return await _uow.SaveAsync() > 0 ? eggGroups.Count : throw new BadRequestException("Something wrong with eggroup list");
        }

        public async Task<bool> DeleteEggGroup(Guid egID)
        {
            var eggGroup = await _uow.EggGroups.GetByIdAsync(egID);

            if (eggGroup == null)
                throw new NotFoundException($"Egg Group with id {egID} not found");

            await _uow.EggGroups.RemoveAsync(eggGroup);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<PagedResult<EggGroupDTO>> GetAllEggGroup(QueryParams queryParams)
        {
            var cacheKey = $"EggGroup:all:page:{queryParams.PageNumber}";

            try
            {
                var cached = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cached))
                {
                    return JsonSerializer.Deserialize<PagedResult<EggGroupDTO>>(cached)!;
                }
            }
            catch
            {
                //ignore cache errors
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
                //ignore cache errors
            }
            return result;
        }

        public async Task<EggGroupDTO?> GetEggGroupById(Guid egID)
        {
            var eggGroup = await _uow.EggGroups.GetByIdAsync(egID);
            if (eggGroup == null)
                throw new NotFoundException($"Ability with id {egID} not found");

            return _mapper.Map<EggGroupDTO>(eggGroup);
        }

        public async Task<bool> UpdateEggGroup(Guid egID, PutEggGroupDTO model)
        {
            var eggGroup = await _uow.EggGroups.GetByIdAsync(egID);

            if (eggGroup == null)
                throw new NotFoundException($"Egg Group with id {egID} not found");

            if (!string.IsNullOrWhiteSpace(model.egName))
                eggGroup.egName = model.egName;

            await _uow.EggGroups.UpdateAsync(eggGroup);

            return await _uow.SaveAsync() > 0;

        }
    }
}
