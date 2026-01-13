using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
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

        public async Task<List<GameVersion>> GetAllAsync()
        {
            return await _context.GameVersion!.AsNoTracking().ToListAsync();
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
    }
}
