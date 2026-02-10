using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class EvolutionChartRepo : BaseRepository<EvolutionChart>, IEvolutionChartRepo
    {
        public EvolutionChartRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(EvolutionChart model)
        {
            await _context.EvolutionChart.AddAsync(model);
        }

        public async Task<List<EvolutionChart>> GetAllAsync()
        {
            return await _context.EvolutionChart!.AsNoTracking().ToListAsync();
        }

        public async Task<EvolutionChart?> GetByIdAsync(Guid evoID)
        {
            return await _context.EvolutionChart!
                .FirstOrDefaultAsync(evo => evo.evoID == evoID);
        }

        public Task RemoveAsync(EvolutionChart model)
        {
            _context.EvolutionChart.Remove(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(EvolutionChart model)
        {
            _context.EvolutionChart!.Update(model);
            return Task.CompletedTask;
        }

        public async Task<EvolutionChart?> FindAsync(Guid pokeID, Guid prePokeID)
        {
            return await _context.EvolutionChart
                .FirstOrDefaultAsync(ec =>
                    ec.pokeID == pokeID &&
                    ec.prePokeID == prePokeID);
        }

        public async Task AddRangeAsync(List<EvolutionChart> evoList)
        {
            await _context.EvolutionChart.AddRangeAsync(evoList);
        }

        public async Task<List<EvolutionChart>> GetExistingPairsAsync( List<Guid> pokeIds, List<Guid> prePokeIds)
        {
            return await _context.EvolutionChart
                .Where(e =>
                    pokeIds.Contains(e.pokeID) &&
                    prePokeIds.Contains(e.prePokeID))
                .ToListAsync();
        }

    }
}
