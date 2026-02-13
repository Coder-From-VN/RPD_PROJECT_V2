using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class EffortValuesService : BaseService, IEffortValuesService
    {
        public EffortValuesService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache,cached)
        {
        }

        public async Task AddEffortValues(PostPokemonsEffortValuesDTO model, Guid pokeID)
        {

            EffortValues newEffortValues = new EffortValues
            {
                evStatName = model.evStatName,
                eValues = model.eValues,
                pokeID = pokeID,
            };

            await _uow.EffortValues.AddAsync(newEffortValues);
        }

        //public async Task<bool> DeleteEffortValues(Guid evID)
        //{
        //    var effortValues = await _uow.EffortValues.GetByIdAsync(evID);
        //    if (effortValues == null)
        //        throw new NotFoundException("Effort Values not found");

        //    await _uow.EffortValues.RemoveAsync(effortValues);

        //    return await _uow.SaveAsync() > 0;
        //}

        public async Task UpdateEffortValues(Guid pokeID, ICollection<PutEffortValuesDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Pokemons with id {pokeID} not found");

            var evLookup = model.ToDictionary(m => m.evStatName);

            foreach (var ev in pokemon.EffortValues)
            {
                if (evLookup.TryGetValue(ev.evStatName, out var dto))
                {
                    ev.eValues = dto.eValues;
                }
            }
        }
    }
}
