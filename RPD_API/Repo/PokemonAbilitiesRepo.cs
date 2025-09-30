using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonAbilitiesRepo : IPokemonAbilitiesRepo
    {
        private readonly rpdDbContext _context;

        public PokemonAbilitiesRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<bool> AddPokemonAbilities(PostPokemonAbilitiesDTO model, Guid pokeID)
        {
            var abIdCheck = await _context.Abilities!
                .SingleOrDefaultAsync(ab => ab.abID == model.abID);
            var pokeIdCheck = await _context.Pokemons!
                .SingleOrDefaultAsync(p => p.pokeID == pokeID);
            if (abIdCheck == null || pokeIdCheck == null)
                return false;
            PokemonAbilities newPokemonAbilities = new PokemonAbilities
            {
                abID = model.abID,
                pokeID = pokeID,
                Abilities = abIdCheck,
                paHiddenCheck = model.paHiddenCheck,
                Pokemons = pokeIdCheck
            };
            _context.PokemonAbilities!.Add(newPokemonAbilities);
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> DeleteEffortValues(Guid pokeID, Guid abID)
        {
            var entry = _context.PokemonAbilities!
                .SingleOrDefault(pb => pb.abID == abID && pb.pokeID == pokeID);
            if (entry != null)
            {
                _context.PokemonAbilities!.Remove(entry);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }
    }
}
