using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class EvolutionChartService : BaseService, IEvolutionChartService
    {
        public EvolutionChartService(IUnitOfWorkRepo uow, IMapper mapper)
        : base(uow, mapper)
        {
        }

        public async Task<bool> PostEvolutionChart(PostEvolutionChartDTO model)
        {
            var pokeCheck = await _uow.Pokemons.GetByIdAsync(model.pokeID);
            var prePokeIdCheck = await _uow.Pokemons.GetByIdAsync(model.prePokeID);
            if (pokeCheck == null || prePokeIdCheck == null)
                return false;

            var evolution = _mapper.Map<EvolutionChart>(model);

            await _uow.EvolutionCharts.AddAsync(evolution);

            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeleteEvolutionChart(Guid pokeID, Guid prePokeID)
        {
            var entry = await _uow.EvolutionCharts.FindAsync(pokeID, prePokeID);
            if (entry == null)
                return false;

            _uow.EvolutionCharts.RemoveAsync(entry);

            return await _uow.SaveAsync() > 0;
        }
    }
}
