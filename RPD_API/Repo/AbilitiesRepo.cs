using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

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

        public async Task<List<Abilities>> GetAllAsync()
        {
            return await _context.Abilities!.AsNoTracking().ToListAsync();
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

    }
}
