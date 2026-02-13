using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Repo.IRepo;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace RPD_API.Repo
{
    public class EggGroupRepo : BaseRepository<EggGroup>, IEggGroupRepo
    {
        public EggGroupRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(EggGroup model)
        {
            await _context.EggGroup.AddAsync(model);
        }

        public async Task AddRangeAsync(List<EggGroup> eggGroups)
        {
            await _context.EggGroup.AddRangeAsync(eggGroups);
        }

        public async Task<PagedResult<EggGroup>> GetAllAsync(QueryParams queryParams)
        {
            var query = _context.EggGroup!
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.ToLower();
                query = query.Where(eg =>
                    eg.egName.ToLower().Contains(search)
                    );
            }

            query = queryParams.SortBy?.ToLower() switch
            {
                "egName" => queryParams.SortOrder == "desc"
                    ? query.OrderByDescending(a => a.egName)
                    : query.OrderBy(a => a.egName),

                _ => query.OrderBy(a => a.egName)
            };

            return await ToPagedResultAsync(query, queryParams);
        }

        public async Task<EggGroup?> GetByIdAsync(Guid egID)
        {
            return await _context.EggGroup!
                .FirstOrDefaultAsync(ab => ab.egID == egID);
        }

        public Task RemoveAsync(EggGroup model)
        {
            _context.EggGroup.Remove(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(EggGroup model)
        {
            _context.EggGroup!.Update(model);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByNameAsync(string egName)
        {
            return await _context.EggGroup!
                .AnyAsync(ab => ab.egName == egName);
        }

        public async Task<List<string>> GetExistingNamesAsync(List<string> names)
        {
            return await _context.EggGroup
                .Where(a => names.Contains(a.egName))
                .Select(a => a.egName)
                .ToListAsync();
        }

        public async Task<bool> ExistsByIdAsync(Guid egID)
        {
            return await _context.EggGroup!
                .AnyAsync(eg => eg.egID == egID);
        }
    }
}
