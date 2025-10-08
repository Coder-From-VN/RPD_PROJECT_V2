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

        public async Task<bool> UpdatePokemonMove(Guid pokeID, ICollection<PutPokemonMoveDTO> model)
        {
            var pokemon = await _context.Pokemons
       .Include(p => p.PokemonMove)
       .FirstOrDefaultAsync(p => p.pokeID == pokeID);

            if (pokemon == null)
                return false;

            // Remove moves that are no longer in the DTO list
            var toRemove = pokemon.PokemonMove
                .Where(m => !model.Any(dto => dto.moveID == m.moveID))
                .ToList();
            _context.RemoveRange(toRemove);

            // Update existing and add new
            foreach (var dto in model)
            {
                var existing = pokemon.PokemonMove.FirstOrDefault(m => m.moveID == dto.moveID);

                if (existing != null)
                {
                    // Update existing move info
                    existing.pmLearnMethod = dto.pmLearnMethod;
                    existing.pmLearnLevel = dto.pmLearnLevel;
                }
                else
                {
                    // Add new move record
                    pokemon.PokemonMove.Add(new PokemonMove
                    {
                        pokeID = pokeID,
                        moveID = dto.moveID,
                        pmLearnMethod = dto.pmLearnMethod,
                        pmLearnLevel = dto.pmLearnLevel
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
