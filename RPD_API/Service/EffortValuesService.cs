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

        //call at pokemonaplication
        public async Task EffortValuesAddOn(Guid pokeID, PostPokemonsEffortValuesDTO model)
        {

            EffortValues newEffortValues = new EffortValues
            {
                evStatName = model.evStatName,
                eValues = model.eValues,
                pokeID = pokeID,
            };

            await _uow.EffortValues.AddAsync(newEffortValues);
        }

        public async Task<bool> DeleteEffortValues(Guid pokeID, Guid evID)
        {
            var effortValues = await _uow.EffortValues.GetByIdAsync(evID);

            if (effortValues == null || effortValues.pokeID != pokeID)
                throw new NotFoundException("Effort Values not found for this Pokemon");

            await _uow.EffortValues.RemoveAsync(effortValues);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> UpdateEffortValues(Guid pokeID, ICollection<PutEffortValuesDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetPokemonWithEVAsync(pokeID);

            if (pokemon == null)
                throw new NotFoundException($"Pokemon with id {pokeID} not found");

            var evLookup = model.ToDictionary(
                m => m.evStatName.Trim().ToLower(),
                m => m.eValues
            );

            foreach (var ev in pokemon.EffortValues)
            {
                var key = ev.evStatName.Trim().ToLower();

                if (evLookup.TryGetValue(key, out var newValue))
                {
                    ev.eValues = newValue;
                }
            }

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> PostEffortValues(Guid pokeID, PostPokemonsEffortValuesDTO model)
        {

            if (!await _uow.Pokemons.ExistsByPokemonByIdAsync(pokeID))
                throw new BadRequestException($"Pokemon Id {pokeID} Not Exist");

            var newEV= _mapper.Map<EffortValues>(model);
            newEV.pokeID = pokeID;

            await _uow.EffortValues.AddAsync(newEV);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }
    }
}
