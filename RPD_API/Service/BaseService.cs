using AutoMapper;
using RPD_API.DTO;
using RPD_API.Pagination;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWorkRepo _uow;
        protected readonly IMapper _mapper;

        protected BaseService(IUnitOfWorkRepo uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
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
