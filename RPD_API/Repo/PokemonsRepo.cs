using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonsRepo : BaseRepository<Pokemons>, IPokemonsRepo
    {

        public PokemonsRepo(rpdDbContext context) : base(context)
        {

        }

        public async Task AddAsync(Pokemons model)
        {
            await _context.Pokemons.AddAsync(model);
        }

        public async Task<bool> ExistsByNationalNumberAsync(int pokeNationalNumber)
        {
            return await _context.Pokemons!
                .AnyAsync(p => p.pokeNationalNumber == pokeNationalNumber);
        }

        public async Task<PagedResult<Pokemons>> GetAllAsync(QueryParams queryParams)
        {
            var query = _context.Pokemons
                    .Include(p => p.ImageLink)
                    .Include(p => p.PokemonType)
                    .ThenInclude(pt => pt.Types)
                    .AsNoTracking()
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.ToLower();
                query = query.Where(poke =>
                    poke.pokeName.ToLower().Contains(search) ||
                    poke.pokeDescription.ToLower().Contains(search) ||
                    poke.pokeNationalNumber.ToString().ToLower().Contains(search)
                    );
            }

            query = queryParams.SortBy?.ToLower() switch
            {
                "pokeNationalNumber" => queryParams.SortOrder == "desc"
                    ? query.OrderByDescending(poke => poke.pokeNationalNumber)
                    : query.OrderBy(poke => poke.pokeNationalNumber),

                _ => query.OrderBy(poke => poke.pokeNationalNumber)
            };

            return await ToPagedResultAsync(query, queryParams);
        }

        public async Task<Pokemons?> GetByIdAsync(Guid pokeID)
        {
            return await _context.Pokemons.AsNoTracking()
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
        }

        public Task RemoveAsync(Pokemons model)
        {
            _context.Pokemons!.Remove(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Pokemons model)
        {
            _context.Pokemons!.Update(model);
            return Task.CompletedTask;
        }

        public async Task AddRangeAsync(List<Pokemons> pokeList)
        {
            await _context.Pokemons.AddRangeAsync(pokeList);
        }

        public async Task<List<int>> GetExistingpokeNationalNumberAsync(List<int> pokeNationalNumber)
        {
            return await _context.Pokemons
                .Where(p => pokeNationalNumber.Contains(p.pokeNationalNumber))
                .Select(p => p.pokeNationalNumber)
                .ToListAsync();
        }

        public async Task<bool> ExistsByPokemonByIdAsync(Guid pokeID)
        {
            return await _context.Pokemons!.AnyAsync(p => p.pokeID == pokeID);
        }

        //get with Abilities
        public async Task<Pokemons?> GetPokemonWithAbilitiesAsync(Guid pokeID)
        {
            return await _context.Pokemons
                .Include(p => p.PokemonAbilities)
                .FirstOrDefaultAsync(p => p.pokeID == pokeID);
        }
        //get with EggGroup
        public async Task<Pokemons?> GetPokemonWithEggGroupsAsync(Guid pokeID)
        {
            return await _context.Pokemons
                .Include(p => p.PokemonEggGroup)
                .FirstOrDefaultAsync(p => p.pokeID == pokeID);
        }
        //get with Gameversion
        public async Task<Pokemons?> GetPokemonWithGameVersionAsync(Guid pokeID)
        {
            return await _context.Pokemons
                .Include(p => p.PokemonGameVersion)
                .FirstOrDefaultAsync(p => p.pokeID == pokeID);
        }
        //get with Stats
        public async Task<Pokemons?> GetPokemonWithStatsAsync(Guid pokeID)
        {
            return await _context.Pokemons
                .Include(p => p.PokemonStats)
                .FirstOrDefaultAsync(p => p.pokeID == pokeID);
        }

        //get with Stats
        public async Task<Pokemons?> GetPokemonWithTypesAsync(Guid pokeID)
        {
            return await _context.Pokemons
                .Include(p => p.PokemonType)
                .FirstOrDefaultAsync(p => p.pokeID == pokeID);
        }

    }
}
