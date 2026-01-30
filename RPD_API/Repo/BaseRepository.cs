using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public abstract class BaseRepository<T> : IBaseRepository
    where T : class
    {
        protected readonly rpdDbContext _context;

        protected BaseRepository(rpdDbContext context)
        {
            _context = context;
        }

        protected async Task<PagedResult<T>> ToPagedResultAsync(
            IQueryable<T> query,
            QueryParams queryParams)
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip(queryParams.Skip)
                .Take(queryParams.PageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                PageNumber = queryParams.PageNumber,
                PageSize = queryParams.PageSize,
                TotalCount = totalCount,
                Items = items
            };
        }
    }
}
