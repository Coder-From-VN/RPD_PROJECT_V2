using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonGameVersionRepo : IPokemonGameVersionRepo
    {
        private readonly rpdDbContext _context;

        public PokemonGameVersionRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<bool> AddPokemonGameVersion(PostPokemonGameVersionDTO model, Guid pokeID)
        {
            var gameVersionCheck = await _context.GameVersion!
                .SingleOrDefaultAsync(gv => gv.gvID == model.gvID);
            var pokeIdCheck = await _context.Pokemons!
                .SingleOrDefaultAsync(p => p.pokeID == pokeID);
            if (gameVersionCheck == null || pokeIdCheck == null)
                return false;
            PokemonGameVersion newPokemonGameVersion = new PokemonGameVersion
            {
                gvID = model.gvID,
                GameVersion = gameVersionCheck,
                pokeID = pokeID,
                Pokemons = pokeIdCheck,
                pgvDexNumber = model.pgvDexNumber,
                pgvEntries = model.pgvEntries
            };
            _context.PokemonGameVersion!.Add(newPokemonGameVersion);
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> DeletePokemonGameVersion(Guid pokeID, Guid gvID)
        {
            var entry = _context.PokemonGameVersion!
                .SingleOrDefault(pgv => pgv.gvID == gvID && pgv.pokeID == pokeID);
            if (entry != null)
            {
                _context.PokemonGameVersion!.Remove(entry);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }

        public async Task<bool> UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonGameVersionDTO> model)
        {
            var pokemon = await _context.Pokemons
        .Include(p => p.PokemonGameVersion)
        .FirstOrDefaultAsync(p => p.pokeID == pokeID);

            if (pokemon == null)
                return false;

            foreach (var version in pokemon.PokemonGameVersion)
            {
                var dto = model.FirstOrDefault(m => m.gvID == version.gvID);
                if (dto != null)
                {
                    version.pgvDexNumber = dto.pgvDexNumber;
                    version.pgvEntries = dto.pgvEntries;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
