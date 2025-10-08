using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonStatsRepo : IPokemonStatsRepo
    {
        private readonly rpdDbContext _context;

        public PokemonStatsRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<bool> AddPokemonStats(PostPokemonStatsDTO model, Guid pokeID)
        {
            var statsCheck = await _context.StatType!
               .SingleOrDefaultAsync(st => st.stID == model.stID);
            var pokeCheck = await _context.Pokemons!
                .SingleOrDefaultAsync(p => p.pokeID == pokeID);
            if (statsCheck == null || pokeCheck == null)
                return false;
            PokemonStats newPokemonStats = new PokemonStats
            {
                stID = model.stID,
                pokeID = pokeID,
                Pokemons = pokeCheck,
                StatType = statsCheck,
                Basevalue = model.Basevalue,
                minValue = model.minValue,
                MaxValue = model.MaxValue

            };
            _context.PokemonStats!.Add(newPokemonStats);
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> DeletePokemonStats(Guid pokeID, Guid stID)
        {
            var entry = _context.PokemonStats!
                 .SingleOrDefault(st => st.stID == stID && st.pokeID == pokeID);
            if (entry != null)
            {
                _context.PokemonStats!.Remove(entry);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }

        public async Task<bool> UpdatePokemonStats(Guid pokeID, ICollection<PutPokemonStatsDTO> model)
        {
            var pokemon = await _context.Pokemons
                .Include(p => p.PokemonStats)
                .FirstOrDefaultAsync(p => p.pokeID == pokeID);

            if (pokemon == null)
                return false;

            foreach (var stat in pokemon.PokemonStats)
            {
                var dto = model.FirstOrDefault(m => m.stID == stat.stID);
                if (dto != null)
                {
                    stat.Basevalue = dto.Basevalue;
                    stat.minValue = dto.minValue;
                    stat.MaxValue = dto.MaxValue;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
