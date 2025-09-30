using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonMoveRepo : IPokemonMoveRepo
    {
        private readonly rpdDbContext _context;

        public PokemonMoveRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<bool> AddPokemonMove(PostPokemonMoveDTO model, Guid pokeID)
        {
            var moveCheck = await _context.Move!
                .SingleOrDefaultAsync(m => m.moveID == model.moveID);
            var pokeIdCheck = await _context.Pokemons!
                .SingleOrDefaultAsync(p => p.pokeID == pokeID);
            if (moveCheck == null || pokeIdCheck == null)
                return false;
            PokemonMove newPokemonMove = new PokemonMove
            {
                moveID = model.moveID,
                Move = moveCheck,
                pokeID = pokeID,
                Pokemons = pokeIdCheck,
                pmLearnLevel = model.pmLearnLevel,
                pmLearnMethod = model.pmLearnMethod
            };
            _context.PokemonMove!.Add(newPokemonMove);
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> DeletePokemonMove(Guid pokeID, Guid moveID)
        {
            var entry = _context.PokemonMove!
                .SingleOrDefault(pm => pm.moveID == moveID && pm.pokeID == pokeID);
            if (entry != null)
            {
                _context.PokemonMove!.Remove(entry);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }
    }
}
