using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class EvolutionChartRepo : IEvolutionChartRepo
    {
        private readonly rpdDbContext _context;

        public EvolutionChartRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
        }

        public async Task<bool> AddEvolutionChart(PostEvolutionChartDTO model)
        {
            var pokeCheck = await _context.Pokemons!
                 .SingleOrDefaultAsync(p => p.pokeID == model.pokeID);
            var prePokeIdCheck = await _context.Pokemons!
                .SingleOrDefaultAsync(p => p.pokeID == model.prePokeID);
            if (pokeCheck == null || prePokeIdCheck == null)
                return false;
            EvolutionChart newEvolutionChart = new EvolutionChart
            {
                pokeID = model.pokeID,
                Pokemons = pokeCheck,
                prePokeID = model.prePokeID,
                PrePokemons = prePokeIdCheck,
                evoCondition = model.evoCondition,
            };
            _context.EvolutionChart!.Add(newEvolutionChart);
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> DeleteEvolutionChart(Guid pokeID, Guid prePokeID)
        {
            var entry = _context.EvolutionChart!
                 .SingleOrDefault(ec => ec.pokeID == pokeID && ec.prePokeID == prePokeID);
            if (entry != null)
            {
                _context.EvolutionChart!.Remove(entry);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }
    }
}
