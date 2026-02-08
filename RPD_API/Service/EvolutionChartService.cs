using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

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
