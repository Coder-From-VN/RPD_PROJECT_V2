using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonEggGroupRepo : IPokemonEggGroupRepo
    {
        private readonly rpdDbContext _context;

        public PokemonEggGroupRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<bool> AddPokemonEggGroup(Guid egID, Guid pokeID)
        {
            var eggGroupCheck = await _context.EggGroup!
                .SingleOrDefaultAsync(eg => eg.egID == egID);
            var pokeCheck = await _context.Pokemons!
                .SingleOrDefaultAsync(p => p.pokeID == pokeID);
            if (eggGroupCheck == null || pokeCheck == null)
                return false;
            PokemonEggGroup newPokemonEggGroup = new PokemonEggGroup
            {
                egID = egID,
                pokeID = pokeID,
                Pokemons = pokeCheck,
                EggGroup = eggGroupCheck
            };
            _context.PokemonEggGroup!.Add(newPokemonEggGroup);
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> DeletePokemonEggGroup(Guid egID, Guid pokeID)
        {
            var entry = _context.PokemonEggGroup!
                .SingleOrDefault(peg => peg.egID == egID && peg.pokeID == pokeID);
            if (entry != null)
            {
                _context.PokemonEggGroup!.Remove(entry);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }
    }
}
