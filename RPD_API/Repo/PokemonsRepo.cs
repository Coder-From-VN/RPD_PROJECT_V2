using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonsRepo : IPokemonsRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public PokemonsRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PokemonsDTO> AddPokemons(PostPokemonDTO model)
        {
            var existing = await _context.Pokemons!.SingleOrDefaultAsync(m => m.pokeNationalNumber == model.pokeNationalNumber);

            if (existing != null)
                return null;

            var newPokemons = _mapper.Map<Pokemons>(model);
            _context.Pokemons.Add(newPokemons);

            var saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                return _mapper.Map<PokemonsDTO>(newPokemons);
            }
            return null;
        }

        public async Task<bool> DeletePokemons(Guid pokeID)
        {
            var pokemons = _context.Pokemons!.SingleOrDefault(b => b.pokeID == pokeID);
            if (pokemons != null)
            {
                _context.Pokemons!.Remove(pokemons);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }

        public async Task<List<PokemonsDTO>> GetAllPokemons()
        {
            //add include
            var pokemons = await _context.Pokemons.Include(m => m.GrowthRate)
                                                  .Include(img => img.ImageLink)
                                                  .Include(ev => ev.EffortValues)
                                                  .Include(ps => ps.PokemonStats).ThenInclude(s => s.StatType)
                                                  .Include(pgv => pgv.PokemonGameVersion).ThenInclude(gv => gv.GameVersion)
                                                  .Include(pa => pa.PokemonAbilities).ThenInclude(a => a.Abilities)
                                                  .Include(eg => eg.PokemonEggGroup).ThenInclude(e => e.EggGroup)
                                                  .Include(pt => pt.PokemonType).ThenInclude(t => t.Types)
                                                  .Include(pm => pm.PokemonMove).ThenInclude(pt => pt.Move).ThenInclude(t => t.Types)
                                                  .ToListAsync();
            return _mapper.Map<List<PokemonsDTO>>(pokemons);
        }

        public async Task<PokemonsDTO> GetPokemonsById(Guid pokeID)
        {
            var pokemons = await _context.Pokemons.FirstOrDefaultAsync(p => p.pokeID == pokeID);
            return _mapper.Map<PokemonsDTO>(pokemons);
        }

        public async Task<bool> UpdatePokemons(Guid pokeID, PokemonsDTO model)
        {
            if (pokeID == model.pokeID)
            {
                var updatePokemons = _mapper.Map<Pokemons>(model);
                _context.Pokemons!.Update(updatePokemons);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }

        public async Task<Pokemons> FindPokemonsById(Guid pokeID)
        {
            var pokemons = await _context.Pokemons.Include(m => m.GrowthRate)
                                                  .Include(img => img.ImageLink)
                                                  .Include(ev => ev.EffortValues)
                                                  .Include(ps => ps.PokemonStats)
                                                    .ThenInclude(s => s.StatType)
                                                  .Include(pgv => pgv.PokemonGameVersion)
                                                    .ThenInclude(gv => gv.GameVersion)
                                                  .Include(pa => pa.PokemonAbilities)
                                                    .ThenInclude(a => a.Abilities)
                                                  .Include(eg => eg.PokemonEggGroup)
                                                    .ThenInclude(e => e.EggGroup)
                                                  .Include(pt => pt.PokemonType)
                                                    .ThenInclude(t => t.Types)
                                                  .Include(pm => pm.PokemonMove)
                                                    .ThenInclude(pt => pt.Move)
                                                    .ThenInclude(t => t.Types)
                                                  .FirstOrDefaultAsync(p => p.pokeID == pokeID);
            return pokemons;
        }
    }
}
