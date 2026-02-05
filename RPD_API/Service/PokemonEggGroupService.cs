using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonEggGroupService : BaseService, IPokemonEggGroupService
    {
        public PokemonEggGroupService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<bool> AddPokemonEggGroup(Guid egID, Guid pokeID)
        {
            var eggGroupCheck = await _uow.EggGroups.GetByIdAsync(egID);
            var pokeCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (eggGroupCheck == null || pokeCheck == null)
                return false;

            var exists = await _uow.PokemonEggGroups.GetLinkAsync(pokeID, egID);
            if (exists != null)
                return false;

            PokemonEggGroup newPokemonEggGroup = new PokemonEggGroup
            {
                egID = egID,
                pokeID = pokeID,
                Pokemons = pokeCheck,
                EggGroup = eggGroupCheck
            };

            await _uow.PokemonEggGroups.AddAsync(newPokemonEggGroup);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeletePokemonEggGroup(Guid egID, Guid pokeID)
        {
            var entry = await _uow.PokemonEggGroups.GetLinkAsync(pokeID, egID);
            if (entry == null)
                return false;

            await _uow.PokemonEggGroups.RemoveAsync(entry);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdatePokemonEggGroup(Guid pokeID, ICollection<PutPokemonEggGroupDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                return false;

            // Remove any egg groups not in the new list
            var existingLinks = pokemon.PokemonEggGroup.ToList();
            foreach (var link in existingLinks)
                await _uow.PokemonEggGroups.RemoveAsync(link);

            // Add new egg groups that aren’t already linked
            foreach (var dto in model)
            {
                var abilityExists = await _uow.EggGroups.GetByIdAsync(dto.egID);
                if (abilityExists == null)
                    return false;

                pokemon.PokemonEggGroup.Add(new PokemonEggGroup
                {
                    pokeID = pokeID,
                    egID = dto.egID
                });

            }
            return await _uow.SaveAsync() > 0;
        }
    }
}
