
using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Repo.IRepo;
using System.Xml.Linq;

namespace RPD_API.Repo
{
    public class GameVersionRepo : BaseRepository<GameVersion>, IGameVersionRepo
    {
        public GameVersionRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(GameVersion model)
        {
            await _context.GameVersion.AddAsync(model);
        }

        public async Task<PagedResult<GameVersion>> GetAllAsync(QueryParams queryParams)
        {
            var query = _context.GameVersion!
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.ToLower();
                query = query.Where(a =>
                    a.gvName.ToLower().Contains(search) ||
                    a.gvGen.ToString().Contains(search)
                    );
            }

            query = queryParams.SortBy?.ToLower() switch
            {
                "gvGen" => queryParams.SortOrder == "desc"
                    ? query.OrderByDescending(a => a.gvGen)
                    : query.OrderBy(a => a.gvGen),

                _ => query.OrderBy(a => a.gvGen)
            };

            return await ToPagedResultAsync(query, queryParams);
        }

        public async Task<GameVersion?> GetByIdAsync(Guid gvID)
        {
            return await _context.GameVersion!
                .FirstOrDefaultAsync(ab => ab.gvID == gvID);
        }

        public Task UpdateAsync(GameVersion model)
        {
            _context.GameVersion!.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(GameVersion model)
        {
            _context.GameVersion.Remove(model);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByNameAsync(string gvName)
        {
            return await _context.GameVersion!
                .AnyAsync(gv => gv.gvName == gvName);
        }

        public async Task AddRangeAsync(List<GameVersion> gameVersions)
        {
            await _context.GameVersion.AddRangeAsync(gameVersions);
        }

        public async Task<List<string>> GetExistingNamesAsync(List<string> names)
        {
            return await _context.GameVersion
                .Where(gv => names.Contains(gv.gvName))
                .Select(gv => gv.gvName)
                .ToListAsync();
        }

        public async Task<bool> ExistsByIdAsync(Guid gvID)
        {
            return await _context.GameVersion!
                .AnyAsync(gv => gv.gvID == gvID);
        }

    }
}
