using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class EffortValuesRepo : BaseRepository<EffortValues>, IEffortValuesRepo
    {
        public EffortValuesRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(EffortValues model)
        {
            await _context.EffortValues.AddAsync(model);
        }

        public async Task<List<EffortValues>> GetAllAsync()
        {
            return await _context.EffortValues!.AsNoTracking().ToListAsync();
        }

        public async Task<EffortValues?> GetByIdAsync(Guid evID)
        {
            return await _context.EffortValues!
                .FirstOrDefaultAsync(ab => ab.evID == evID);
        }

        public Task UpdateAsync(EffortValues model)
        {
            _context.EffortValues!.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(EffortValues model)
        {
            _context.EffortValues.Remove(model);
            return Task.CompletedTask;
        }


    }
}
