using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonEggGroupRepo : BaseRepository<PokemonEggGroup>, IPokemonEggGroupRepo
    {
        public PokemonEggGroupRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(PokemonEggGroup model)
        {
            await _context.PokemonEggGroup.AddAsync(model);
        }

        public async Task<PokemonEggGroup?> GetLinkAsync(Guid pokeID, Guid egID)
        {
            return await _context.PokemonEggGroup
                 .FirstOrDefaultAsync(eg => eg.egID == egID && eg.pokeID == pokeID);
        }

        public Task RemoveAsync(PokemonEggGroup model)
        {
            _context.PokemonEggGroup.Remove(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(PokemonEggGroup model)
        {
            _context.PokemonEggGroup!.Update(model);
            return Task.CompletedTask;
        }
    }
}
