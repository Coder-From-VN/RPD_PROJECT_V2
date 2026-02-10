using AutoMapper;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
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
        public EvolutionChartService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
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

            var evoDtos = csv.GetRecords<PostEvolutionChartDTO>().ToList();

            // 🔴 [ADD] Basic validation + normalize
            var normalizedDtos = evoDtos
                .Where(x =>
                    x.pokeID != Guid.Empty &&
                    x.prePokeID != Guid.Empty &&
                    x.pokeID != x.prePokeID &&
                    !string.IsNullOrWhiteSpace(x.evoCondition))
                .Select(x => new PostEvolutionChartDTO
                {
                    pokeID = x.pokeID,
                    prePokeID = x.prePokeID,
                    evoCondition = x.evoCondition.Trim()
                })
                // 🔴 [ADD] Remove duplicate rows inside CSV
                .GroupBy(x => new { x.pokeID, x.prePokeID })
                .Select(g => g.First())
                .ToList();

            if (!normalizedDtos.Any())
                return 0;

            // 🔴 [ADD] Check existing evolutions in DB
            var existingPairs = await _uow.EvolutionCharts
                .GetExistingPairsAsync(
                    normalizedDtos.Select(x => x.pokeID).ToList(),
                    normalizedDtos.Select(x => x.prePokeID).ToList()
                );

            var newDtos = normalizedDtos
                .Where(x => !existingPairs.Any(e =>
                    e.pokeID == x.pokeID &&
                    e.prePokeID == x.prePokeID))
                .ToList();

            if (!newDtos.Any())
                return 0;

            var evolutions = _mapper.Map<List<EvolutionChart>>(newDtos);

            await _uow.EvolutionCharts.AddRangeAsync(evolutions);

            return await _uow.SaveAsync() > 0
                ? evolutions.Count
                : throw new BadRequestException("Something went wrong with EvolutionChart import");
        }

        public async Task<bool> DeleteEvolutionChart(Guid pokeID, Guid prePokeID)
        {
            var entry = await _uow.EvolutionCharts.FindAsync(pokeID, prePokeID);
            if (entry == null)
                throw new NotFoundException("EvolutionCharts not found");

            await _uow.EvolutionCharts.RemoveAsync(entry);

            return await _uow.SaveAsync() > 0;
        }
    }
}
