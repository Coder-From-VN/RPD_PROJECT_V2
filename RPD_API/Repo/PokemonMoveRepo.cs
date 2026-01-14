using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
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
