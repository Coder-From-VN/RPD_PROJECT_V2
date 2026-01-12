using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
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

        public async Task<List<EggGroup>> GetAllAsync()
        {
            return await _context.EggGroup!.AsNoTracking().ToListAsync();
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
    }
}
