using Google.Apis.Util;
using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class MoveRepo : BaseRepository<Move>, IMoveRepo
    {

        public MoveRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Move model)
        {
            await _context.Move.AddAsync(model);
        }

        public async Task<PagedResult<Move>> GetAllAsync(QueryParams queryParams)
        {
            var query = _context.Move
                .Include(m => m.Types)
                .AsNoTracking()
                .AsQueryable();


            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.ToLower();
                query = query.Where(m =>
                    m.moveName.ToLower().Contains(search) ||
                    m.moveDescription.ToLower().Contains(search)
                    );
            }

            query = queryParams.SortBy?.ToLower() switch
            {
                "moveName" => queryParams.SortOrder == "desc"
                    ? query.OrderByDescending(a => a.moveName)
                    : query.OrderBy(a => a.moveName),

                "movePower" => queryParams.SortOrder == "desc"
                    ? query.OrderByDescending(a => a.movePower)
                    : query.OrderBy(a => a.movePower),

                _ => query.OrderBy(a => a.moveName)
            };

            return await ToPagedResultAsync(query, queryParams);
        }

        public async Task<Move?> GetByIdAsync(Guid moveID)
        {
            return await _context.Move
                .Include(m => m.Types)
                .FirstOrDefaultAsync(m => m.moveID == moveID);
        }

        public Task UpdateAsync(Move model)
        {
            _context.Move!.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(Move model)
        {
            _context.Move.Remove(model);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByNameAsync(string moveName)
        {
            return await _context.Move!
                .AnyAsync(move => move.moveName == moveName);
        }

        public async Task AddRangeAsync(List<Move> mList)
        {
            await _context.Move.AddRangeAsync(mList);
        }

        public async Task<List<string>> GetExistingNamesAsync(List<string> names)
        {
            return await _context.Move
                .Where(m => names.Contains(m.moveName))
                .Select(m => m.moveName)
                .ToListAsync();
        }

        public async Task<List<Move>> GetByIdsAsync(List<Guid> ids)
        {
            return await _context.Move
                .Where(m => ids.Contains(m.moveID))
                .ToListAsync();
        }

        public async Task<bool> ExistsByIdAsync(Guid moveID)
        {
            return await _context.Move!
               .AnyAsync(move => move.moveID == moveID);
        }
    }
}
