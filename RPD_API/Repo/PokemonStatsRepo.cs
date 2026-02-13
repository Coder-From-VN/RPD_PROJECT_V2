using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonStatsRepo : BaseRepository<PokemonStats>, IPokemonStatsRepo
    {
        public PokemonStatsRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(PokemonStats model)
        {
            await _context.PokemonStats.AddAsync(model);
        }

        public async Task<PokemonStats?> GetLinkAsync(Guid pokeID, Guid stID)
        {
            return await _context.PokemonStats
                .FirstOrDefaultAsync(ps => ps.stID == stID && ps.pokeID == pokeID);
        }

        public Task RemoveAsync(PokemonStats model)
        {
            _context.PokemonStats.Remove(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(PokemonStats model)
        {
            _context.PokemonStats!.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveRange(IEnumerable<PokemonStats> entities)
        {
            _context.PokemonStats.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task AddRangeAsync(List<PokemonStats> pokeGVs)
        {
            await _context.PokemonStats.AddRangeAsync(pokeGVs);
        }
    }
}
