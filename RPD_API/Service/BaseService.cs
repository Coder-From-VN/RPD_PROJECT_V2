using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.Pagination;
using RPD_API.UnitOfWork;

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
    }
}
