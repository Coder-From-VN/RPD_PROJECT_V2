using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class GrowthRateRepo : BaseRepository<GrowthRate>, IGrowthRateRepo
    {
        public GrowthRateRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(GrowthRate model)
        {
            await _context.GrowthRate.AddAsync(model);
        }

        public async Task<List<GrowthRate>> GetAllAsync()
        {
            return await _context.GrowthRate!.AsNoTracking().ToListAsync();
        }

        public async Task<GrowthRate?> GetByIdAsync(Guid growthRateID)
        {
            return await _context.GrowthRate!
                .FirstOrDefaultAsync(gr => gr.growthRateID == growthRateID);
        }

        public Task UpdateAsync(GrowthRate model)
        {
            _context.GrowthRate!.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(GrowthRate model)
        {
            _context.GrowthRate.Remove(model);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByNameAsync(string grName)
        {
            return await _context.GrowthRate!
                .AnyAsync(gr => gr.grName == grName);
        }
    }
}
