using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.Text.Json;

namespace RPD_API.Service
{
    public class GrowthRateService : BaseService, IGrowthRateService
    {
        public GrowthRateService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<GrowthRateDTO?> AddGrowthRate(PostGrowthRateDTO model)
        {
            if (await _uow.GrowthRates.ExistsByNameAsync(model.grName))
                throw new BadRequestException("GrowthRates name already exists");

            var newGrowthRate = _mapper.Map<GrowthRate>(model);
            await _uow.GrowthRates.AddAsync(newGrowthRate);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<GrowthRateDTO?>(newGrowthRate) : null;
        }

        public async Task<bool> DeleteGrowthRate(Guid growthRateID)
        {
            var growthRate = await _uow.GrowthRates.GetByIdAsync(growthRateID);
            if (growthRate == null)
                throw new NotFoundException($"GrowthRates with id {growthRateID} not found");

            await _uow.GrowthRates.RemoveAsync(growthRate);

            return await _uow.SaveAsync() > 0;
        }

        public async Task<PagedResult<GrowthRateDTO>> GetAllGrowthRate(QueryParams queryParams)
        {
            var cacheKey = $"GrowthRate:all:page:{queryParams.PageNumber}";
            try
            {
                var cached = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cached))
                {
                    return JsonSerializer.Deserialize<PagedResult<GrowthRateDTO>>(cached)!;
                }
            }
            catch (Exception ex)
            {
                // Optional: log cache read failure

            }
            var result = await GetPagedAsync<GrowthRate, GrowthRateDTO>(
                queryParams,
                _uow.GrowthRates.GetAllAsync
            );

            try
            {
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
                    });
            }
            catch (Exception ex)
            {
                // Optional: log cache write failure
            }
            return result;
        }

        public async Task<GrowthRateDTO> GetGrowthRateById(Guid growthRateID)
        {
            var growthRate = await _uow.GrowthRates.GetByIdAsync(growthRateID);
            if (growthRate == null)
                throw new NotFoundException($"GrowthRates with id {growthRateID} not found");
            return _mapper.Map<GrowthRateDTO>(growthRate);
        }

        public async Task<bool> UpdateGrowthRate(Guid growthRateID, PutGrowthRateDTO model)
        {
            var growthRate = await _uow.GrowthRates.GetByIdAsync(growthRateID);
            if (growthRate == null)
                throw new NotFoundException($"GrowthRates with id {growthRateID} not found");

            if (!string.IsNullOrWhiteSpace(model.grName))
                growthRate.grName = model.grName;
            if (model.grTotalExp != 0)
                growthRate.grTotalExp = model.grTotalExp;

            await _uow.GrowthRates.UpdateAsync(growthRate);

            return await _uow.SaveAsync() > 0;
        }
    }
}
