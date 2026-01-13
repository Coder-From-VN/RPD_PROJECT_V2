using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class StatTypeRepo : BaseRepository<StatType>, IStatTypeRepo
    {
        public StatTypeRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(StatType model)
        {
            await _context.StatType.AddAsync(model);
        }

        public async Task<List<StatType>> GetAllAsync()
        {
            return await _context.StatType!.AsNoTracking().ToListAsync();
        }

        public async Task<StatType?> GetByIdAsync(Guid stID)
        {
            return await _context.StatType!
                .FirstOrDefaultAsync(st => st.stID == stID);
        }

        public Task UpdateAsync(StatType model)
        {
            _context.StatType!.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(StatType model)
        {
            _context.StatType.Remove(model);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByNameAsync(string stName)
        {
            return await _context.StatType!
                .AnyAsync(st => st.stName == stName);
        }
    }
}
