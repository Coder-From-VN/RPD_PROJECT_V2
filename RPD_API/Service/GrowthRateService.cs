using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System;

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
                return null;

            var newGrowthRate = _mapper.Map<GrowthRate>(model);
            await _uow.GrowthRates.AddAsync(newGrowthRate);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<GrowthRateDTO?>(newGrowthRate) : null;
        }

        public async Task<bool> DeleteGrowthRate(Guid growthRateID)
        {
            var growthRate = await _uow.GrowthRates.GetByIdAsync(growthRateID);
            if (growthRate == null)
                return false;

            await _uow.GrowthRates.RemoveAsync(growthRate);

            return await _uow.SaveAsync() > 0;
        }

        public Task<PagedResult<GrowthRateDTO>> GetAllGrowthRate(QueryParams queryParams)
        {
            return GetPagedAsync<GrowthRate, GrowthRateDTO>(
                queryParams,
                _uow.GrowthRates.GetAllAsync
            );
        }

        public async Task<GrowthRateDTO> GetGrowthRateById(Guid growthRateID)
        {
            var growthRate = await _uow.GrowthRates.GetByIdAsync(growthRateID);
            return _mapper.Map<GrowthRateDTO>(growthRate);
        }

        public async Task<bool> UpdateGrowthRate(Guid growthRateID, PutGrowthRateDTO model)
        {
            var growthRate = await _uow.GrowthRates.GetByIdAsync(growthRateID);
            if (growthRate == null)
                return false;

            if (!string.IsNullOrWhiteSpace(model.grName))
                growthRate.grName = model.grName;
            if (model.grTotalExp != 0)
                growthRate.grTotalExp = model.grTotalExp;

            await _uow.GrowthRates.UpdateAsync(growthRate);

            return await _uow.SaveAsync() > 0;


        }
    }
}
