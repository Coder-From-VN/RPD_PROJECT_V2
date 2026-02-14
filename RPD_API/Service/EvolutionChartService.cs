using AutoMapper;
using CsvHelper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.Globalization;

namespace RPD_API.Service
{
    public class EvolutionChartService : BaseService, IEvolutionChartService
    {
        public EvolutionChartService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache,cached)
        {
        }

        public async Task<bool> PostEvolutionChart(PostEvolutionChartDTO model)
        {
            var pokeCheck = await _uow.Pokemons.GetByIdAsync(model.pokeID);
            var prePokeIdCheck = await _uow.Pokemons.GetByIdAsync(model.prePokeID);
            if (pokeCheck == null)
                throw new NotFoundException($"Pokemon id {model.pokeID} Not Found");
            if (prePokeIdCheck == null)
                throw new NotFoundException($"PrePokemon id {model.prePokeID} Not Found");

            var evolution = _mapper.Map<EvolutionChart>(model);

            await _uow.EvolutionCharts.AddAsync(evolution);

            return await _uow.SaveAsync() > 0;
        }

        public async Task<int> ImportEvolutionChartAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var dtos = csv.GetRecords<PostEvolutionChartDTO>()
                          .Where(x =>
                              x.pokeID != Guid.Empty &&
                              x.prePokeID != Guid.Empty &&
                              x.pokeID != x.prePokeID)
                          .GroupBy(x => new { x.pokeID, x.prePokeID })
                          .Select(g => g.First())
                          .ToList();

            if (!dtos.Any())
                return 0;

            var entities = _mapper.Map<List<EvolutionChart>>(dtos);

            await _uow.EvolutionCharts.AddRangeAsync(entities);

            await _uow.SaveAsync();

            return entities.Count;
        }



        public async Task<bool> DeleteEvolutionChart(Guid pokeID, Guid prePokeID)
        {
            var entry = await _uow.EvolutionCharts.FindAsync(pokeID, prePokeID);
            if (entry == null)
                throw new NotFoundException("EvolutionCharts not found");

            await _uow.EvolutionCharts.RemoveAsync(entry);

            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateEvolutionChart(Guid pokeID, Guid prePokeID, PutEvolutionChartDTO model)
        {
            var pokemonEC = await _uow.EvolutionCharts.FindAsync(pokeID, prePokeID);
            if (pokemonEC == null)
                throw new NotFoundException("Charts Not Found");

            _mapper.Map(model, pokemonEC);

            //await _uow.PokemonMoves.UpdateAsync(pokemonMove); 
            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
                await _cache.RemoveAsync($"Pokemons:pokeid:{prePokeID}");
            }

            return saved;
        }
    }
}
