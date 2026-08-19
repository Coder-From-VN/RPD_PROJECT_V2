using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.Pagination;
using RPD_API.UnitOfWork;
using Serilog;
using System.Text.Json;

namespace RPD_API.Service
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWorkRepo _uow;
        protected readonly IMapper _mapper;
        protected readonly IDistributedCache _cache;
        protected readonly ICacheService _cached;

        protected BaseService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        {
            _uow = uow;
            _mapper = mapper;
            _cache = cache;
            _cached = cached;
        }

        protected async Task<PagedResult<TDto>> GetPagedAsync<TEntity, TDto>(
        QueryParams queryParams,
        Func<QueryParams, Task<PagedResult<TEntity>>> getPagedFunc,
        int maxPageSize = 50)
        {
            queryParams.PageSize = Math.Min(queryParams.PageSize, maxPageSize);

            var paged = await getPagedFunc(queryParams);

            return new PagedResult<TDto>
            {
                PageNumber = paged.PageNumber,
                PageSize = paged.PageSize,
                TotalCount = paged.TotalCount,
                Items = _mapper.Map<List<TDto>>(paged.Items)
            };
        }

        protected async Task<T> GetOrSetCacheAsync<T>(
            string cacheKey,
            Func<Task<T>> factory,
            TimeSpan expiration)
        {
            try
            {
                var cached = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cached))
                {
                    return JsonSerializer.Deserialize<T>(cached)!;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Cache read failed for key {cacheKey}: {ex}");
            }

            var result = await factory();

            try
            {
                await _cache.SetStringAsync(
                    cacheKey,
                    JsonSerializer.Serialize(result),
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration });
            }
            catch (Exception ex)
            {
                Log.Error($"Cache write failed for key {cacheKey}: {ex}");
            }

            return result;
        }
    }
}
