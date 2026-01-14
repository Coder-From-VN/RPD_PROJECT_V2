using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class EffortValuesService : BaseService, IEffortValuesService
    {
        public EffortValuesService(IUnitOfWorkRepo uow, IMapper mapper)
        : base(uow, mapper)
        {
        }

        public async Task<bool> AddEffortValues(PostPokemonsEffortValuesDTO model, Guid pokeID)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                return false;

            EffortValues newEffortValues = new EffortValues
            {
                evStatName = model.evStatName,
                eValues = model.eValues,
                pokeID = pokemon.pokeID,
                Pokemons = pokemon
            };

            await _uow.EffortValues.AddAsync(newEffortValues);

            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeleteEffortValues(Guid evID)
        {
            var effortValues = await _uow.EffortValues.GetByIdAsync(evID);
            if (effortValues == null)
                return false;

            await _uow.EffortValues.RemoveAsync(effortValues);

            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateEffortValues(Guid pokeID, ICollection<PutEffortValuesDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                return false;

            var evLookup = model.ToDictionary(m => m.evStatName);

            foreach (var ev in pokemon.EffortValues)
            {
                if (evLookup.TryGetValue(ev.evStatName, out var dto))
                {
                    ev.eValues = dto.eValues;
                }
            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
