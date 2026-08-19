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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RPD_API.Service
{
    public class GrowthRateService : BaseService, IGrowthRateService
    {
        public GrowthRateService(
            IUnitOfWorkRepo uow, 
            IMapper mapper, 
            IDistributedCache cache, 
            ICacheService cached)
        : base(uow, mapper, cache, cached)
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

        public async Task<int> ImportGrowthRateAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var growthRateDtos = csv.GetRecords<PostGrowthRateDTO>().ToList();

            var normalizedDtos = growthRateDtos
                .Where(x => !string.IsNullOrWhiteSpace(x.grName))
                .Select(x => new PostGrowthRateDTO
                {
                    grName = x.grName,
                    grTotalExp = x.grTotalExp,
                })
                .GroupBy(x => x.grName.ToLower())
                .Select(g => g.First())
                .ToList();

            var names = normalizedDtos
                .Select(x => x.grName)
                .ToList();

            var existingNames = await _uow.GrowthRates
                .GetExistingNamesAsync(names);

            var newDtos = normalizedDtos
                .Where(x => !existingNames.Contains(x.grName))
                .ToList();

            var growthRates = _mapper.Map<List<GrowthRate>>(newDtos);

            if (!growthRates.Any())
                return 0;

            await _uow.GrowthRates.AddRangeAsync(growthRates);

            return await _uow.SaveAsync() > 0 ? growthRates.Count : throw new BadRequestException("something worng with abilities list");
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
            var cacheKey = $"GrowthRate:all:page:{queryParams.PageNumber}:size:{queryParams.PageSize}";

            return await GetOrSetCacheAsync(
                cacheKey,
                () => GetPagedAsync<GrowthRate, GrowthRateDTO>(queryParams, _uow.GrowthRates.GetAllAsync),
                TimeSpan.FromMinutes(3)
            );
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

            _mapper.Map(model, growthRate);

            await _uow.GrowthRates.UpdateAsync(growthRate);

            return await _uow.SaveAsync() > 0;
        }
    }
}
