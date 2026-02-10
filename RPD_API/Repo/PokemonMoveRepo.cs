using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonMoveRepo : BaseRepository<PokemonMove>, IPokemonMoveRepo
    {
        public PokemonMoveRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(PokemonMove model)
        {
            await _context.PokemonMove.AddAsync(model);
        }

        public async Task AddRangeAsync(List<PokemonMove> moveList)
        {
            await _context.PokemonMove.AddRangeAsync(moveList);
        }

        public async Task<List<PokemonMove>> GetExistingPairsAsync(List<Guid> pokeIds, List<Guid> moveIDs)
        {
            return await _context.PokemonMove
               .Where(e =>
                   pokeIds.Contains(e.pokeID) &&
                   moveIDs.Contains(e.moveID))
               .ToListAsync();
        }

        public async Task<List<PokemonMove>> GetExistingMovesForPokemonAsync(Guid pokeID,List<Guid> moveIds)
        {
            return await _context.PokemonMove
                .Where(pm => pm.pokeID == pokeID && moveIds.Contains(pm.moveID))
                .ToListAsync();
        }

        public async Task<PokemonMove?> GetLinkAsync(Guid pokeID, Guid moveID)
        {
            return await _context.PokemonMove
                .FirstOrDefaultAsync(pm => pm.moveID == moveID && pm.pokeID == pokeID);
        }

        public Task RemoveAsync(PokemonMove model)
        {
            _context.PokemonMove.Remove(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(PokemonMove model)
        {
            _context.PokemonMove!.Update(model);
            return Task.CompletedTask;
        }
    }
}
