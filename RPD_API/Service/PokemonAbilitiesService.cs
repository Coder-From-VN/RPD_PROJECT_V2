using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;


namespace RPD_API.Service
{
    public class PokemonAbilitiesService : BaseService, IPokemonAbilitiesService
    {
        public PokemonAbilitiesService(IUnitOfWorkRepo uow, IMapper mapper)
        : base(uow, mapper)
        {
        }

        public async Task<bool> AddPokemonAbilities(PostPokemonAbilitiesDTO model, Guid pokeID)
        {
            var abIdCheck = await _uow.Abilities.GetByIdAsync(model.abID);
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (abIdCheck == null || pokeIdCheck == null)
                return false;

            var exists = await _uow.PokemonAbilities.GetLinkAsync(pokeID, model.abID);
            if (exists != null)
                return false;

            PokemonAbilities newPokemonAbilities = new PokemonAbilities
            {
                abID = model.abID,
                pokeID = pokeID,
                Abilities = abIdCheck,
                paHiddenCheck = model.paHiddenCheck,
                Pokemons = pokeIdCheck
            };

            await _uow.PokemonAbilities.AddAsync(newPokemonAbilities);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeletePokemonAbilities(Guid pokeID, Guid abID)
        {
            var entry = await _uow.PokemonAbilities.GetLinkAsync(pokeID, abID);
            if (entry == null)
                return false;

            await _uow.PokemonAbilities.RemoveAsync(entry);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonAbilitiesDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                return false;

            var existingLinks = pokemon.PokemonAbilities.ToList();
            foreach (var link in existingLinks)
                await _uow.PokemonAbilities.RemoveAsync(link);

            foreach (var dto in model)
            {
                var abilityExists = await _uow.Abilities.GetByIdAsync(dto.abID);
                if (abilityExists == null)
                    return false;

                pokemon.PokemonAbilities.Add(new PokemonAbilities
                {
                    pokeID = pokeID,
                    abID = dto.abID,
                    paHiddenCheck = dto.paHiddenCheck
                });
            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
