using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Repo.IRepo;
using System.Linq.Expressions;

namespace RPD_API.Repo
{
    public class AbilitiesRepo : BaseRepository<Abilities>, IAbilitiesRepo
    {
        public AbilitiesRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Abilities model)
        {
            await _context.Abilities.AddAsync(model);
        }

        public async Task<PagedResult<Abilities>> GetAllAsync(QueryParams queryParams)
        {
            var query = _context.Abilities!
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.ToLower();
                query = query.Where(a =>
                    a.abName.ToLower().Contains(search) ||
                    a.abDescription.ToLower().Contains(search) ||
                    a.abEffect.ToLower().Contains(search)
                    );
            }

            query = queryParams.SortBy?.ToLower() switch
            {
                "abname" => queryParams.SortOrder == "desc"
                    ? query.OrderByDescending(a => a.abName)
                    : query.OrderBy(a => a.abName),

                _ => query.OrderBy(a => a.abName)
            };

            return await ToPagedResultAsync(query, queryParams);
        }

        public async Task<List<Abilities>> GetPagedAsync()
        {
            return await _context.Abilities.ToListAsync();
        }

        public async Task<Abilities?> GetByIdAsync(Guid abID)
        {
            return await _context.Abilities!
                .FirstOrDefaultAsync(ab => ab.abID == abID);
        }

        public Task UpdateAsync(Abilities model)
        {
            _context.Abilities!.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(Abilities model)
        {
            _context.Abilities.Remove(model);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByNameAsync(string abName)
        {
            return await _context.Abilities!
                .AnyAsync(ab => ab.abName == abName);
        }

        public async Task<bool> ExistsByIdAsync(Guid abID)
        {
            return await _context.Abilities!
                .AnyAsync(ab => ab.abID == abID);
        }


        public async Task<List<string>> GetExistingNamesAsync(List<string> names)
        {
            return await _context.Abilities
                .Where(a => names.Contains(a.abName))
                .Select(a => a.abName)
                .ToListAsync();
        }

        public async Task AddRangeAsync(List<Abilities> abilities)
        {
            await _context.Abilities.AddRangeAsync(abilities);
        }
    }
}
