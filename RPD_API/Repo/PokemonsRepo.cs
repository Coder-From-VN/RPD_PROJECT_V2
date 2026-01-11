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

        public async Task AddPokemons(Pokemons model)
        {
            await _context.Pokemons.AddAsync(model);
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
            var pokemons = await _context.Pokemons
                    .Include(p => p.ImageLink)
                    .Include(p => p.PokemonType)
                    .ThenInclude(pt => pt.Types)
                    .ToListAsync();
            return _mapper.Map<List<PokemonsDTO>>(pokemons);
        }

        public async Task<PokemonDetailDTO> GetPokemonsById(Guid pokeID)
        {
            var pokemons = await _context.Pokemons
                .Include(m => m.GrowthRate)
                .Include(img => img.ImageLink)
                .Include(ev => ev.EffortValues)
                .Include(ps => ps.PokemonStats).ThenInclude(s => s.StatType)
                .Include(pgv => pgv.PokemonGameVersion).ThenInclude(gv => gv.GameVersion)
                .Include(pa => pa.PokemonAbilities).ThenInclude(a => a.Abilities)
                .Include(eg => eg.PokemonEggGroup).ThenInclude(e => e.EggGroup)
                .Include(pt => pt.PokemonType).ThenInclude(t => t.Types)
                .Include(pm => pm.PokemonMove).ThenInclude(pt => pt.Move).ThenInclude(t => t.Types)
                .Include(p => p.EvolutionChart).ThenInclude(e => e.PrePokemons).ThenInclude(p2 => p2.ImageLink)
                .Include(p => p.PreEvolutionChart).ThenInclude(e => e.Pokemons).ThenInclude(p2 => p2.ImageLink)
                .FirstOrDefaultAsync(p => p.pokeID == pokeID);
            if (pokemons == null)
                return null;
            return _mapper.Map<PokemonDetailDTO>(pokemons);
        }

        public async Task<bool> UpdatePokemons(Guid pokeID, PutPokemonDTO model)
        {
            var pokemon = await _context.Pokemons.FirstOrDefaultAsync(p => p.pokeID == pokeID);

            if (pokemon == null)
                return false;

            // Map manually or use AutoMapper here
            pokemon.pokeNationalNumber = model.pokeNationalNumber;
            pokemon.pokeName = model.pokeName;
            pokemon.pokeDescription = model.pokeDescription;
            pokemon.pokeSpecies = model.pokeSpecies;
            pokemon.pokeHeight = model.pokeHeight;
            pokemon.pokeWidth = model.pokeWidth;
            pokemon.pokeCatchRate = model.pokeCatchRate;
            pokemon.pokeBaseFriendship = model.pokeBaseFriendship;
            pokemon.pokeBaseExp = model.pokeBaseExp;
            pokemon.pokeMaleRate = model.pokeMaleRate;
            pokemon.pokeFemaleRate = model.pokeFemaleRate;
            pokemon.pokeEggCycles = model.pokeEggCycles;
            pokemon.pokeState = model.pokeState;
            pokemon.growthRateID = model.growthRateID;

            _context.Pokemons.Update(pokemon);
            await _context.SaveChangesAsync();
            return true;
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

        public async Task<bool> CheckPokemonExited(int pokeNationalNumber)
        {
            return await _context.Pokemons!
                .AnyAsync(p => p.pokeNationalNumber == pokeNationalNumber);
        }
    }
}
