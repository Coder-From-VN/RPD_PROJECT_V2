using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;


namespace RPD_API.Service
{
    public class PokemonAbilitiesService : BaseService, IPokemonAbilitiesService
    {
        public PokemonAbilitiesService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task AddPokemonAbilities(PostPokemonAbilitiesDTO model, Guid pokeID)
        {
            var abIdCheck = await _uow.Abilities.GetByIdAsync(model.abID);
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (abIdCheck == null)
                throw new NotFoundException($"Can't find Abilities id {model.abID}");
            if (abIdCheck == null)
                throw new NotFoundException($"Can't find Pokemons id {pokeID}");

            var exists = await _uow.PokemonAbilities.GetLinkAsync(pokeID, model.abID);
            if (exists != null)
                throw new BadRequestException("Pokemon Alredy have this Abilities");

            PokemonAbilities newPokemonAbilities = new PokemonAbilities
            {
                abID = model.abID,
                pokeID = pokeID,
                Abilities = abIdCheck,
                paHiddenCheck = model.paHiddenCheck,
                Pokemons = pokeIdCheck
            };

            await _uow.PokemonAbilities.AddAsync(newPokemonAbilities);
        }

        //public async Task<bool> DeletePokemonAbilities(Guid pokeID, Guid abID)
        //{
        //    var entry = await _uow.PokemonAbilities.GetLinkAsync(pokeID, abID);
        //    if (entry == null)
        //        throw new NotFoundException("Pokemon Don't have this Abilities");

        //    await _uow.PokemonAbilities.RemoveAsync(entry);
        //    return await _uow.SaveAsync() > 0;
        //}

        public async Task UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonAbilitiesDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Can't find Pokemons id {pokeID}");

            var existingLinks = pokemon.PokemonAbilities.ToList();
            foreach (var link in existingLinks)
                await _uow.PokemonAbilities.RemoveAsync(link);

            foreach (var dto in model)
            {
                var abilityExists = await _uow.Abilities.GetByIdAsync(dto.abID);
                if (abilityExists == null)
                    throw new NotFoundException($"Can't find Abilities id {dto.abID}");

                pokemon.PokemonAbilities.Add(new PokemonAbilities
                {
                    pokeID = pokeID,
                    abID = dto.abID,
                    paHiddenCheck = dto.paHiddenCheck
                });
            }
        }
    }
}
