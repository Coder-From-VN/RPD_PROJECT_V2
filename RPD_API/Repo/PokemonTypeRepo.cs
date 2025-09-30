using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonTypeRepo : IPokemonTypeRepo
    {
        private readonly rpdDbContext _context;

        public PokemonTypeRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<bool> AddPokemonType(Guid typesID, Guid pokeID)
        {
            var typesCheck = await _context.Types!
                .SingleOrDefaultAsync(t => t.typesID == typesID);
            var pokeCheck = await _context.Pokemons!
                .SingleOrDefaultAsync(p => p.pokeID == pokeID);
            if (typesCheck == null || pokeCheck == null)
                return false;
            PokemonType newPokemonType = new PokemonType
            {
                typesID = typesID,
                pokeID = pokeID,
                Pokemons = pokeCheck,
                Types = typesCheck
            };
            _context.PokemonType!.Add(newPokemonType);
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> DeletePokemonType(Guid typesID, Guid pokeID)
        {
            var entry = _context.PokemonType!
                .SingleOrDefault(pt => pt.typesID == typesID && pt.pokeID == pokeID);
            if (entry != null)
            {
                _context.PokemonType!.Remove(entry);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }
    }
}
