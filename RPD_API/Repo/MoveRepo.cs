using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
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

        public async Task<List<Move>> GetAllAsync()
        {
            return await _context.Move
                .Include(m => m.Types)
                .AsNoTracking()
                .ToListAsync();
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

    }
}
