using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
using System.Linq.Expressions;

namespace RPD_API.Repo
{
    public class PokemonAbilitiesRepo : BaseRepository<PokemonAbilities>, IPokemonAbilitiesRepo
    {
        public PokemonAbilitiesRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(PokemonAbilities model)
        {
            await _context.PokemonAbilities.AddAsync(model);
        }

        public async Task<PokemonAbilities?> GetLinkAsync(Guid pokeID, Guid abID)
        {
            return await _context.PokemonAbilities
                .FirstOrDefaultAsync(pb => pb.abID == abID && pb.pokeID == pokeID);
        }

        public Task RemoveAsync(PokemonAbilities model)
        {
            _context.PokemonAbilities.Remove(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(PokemonAbilities model)
        {
            _context.PokemonAbilities!.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveRange(IEnumerable<PokemonAbilities> entities)
        {
            _context.PokemonAbilities.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task AddRangeAsync(List<PokemonAbilities> abilities)
        {
            await _context.PokemonAbilities.AddRangeAsync(abilities);
        }

    }
}
