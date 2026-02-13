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
    public class PokemonAbilitiesService : BaseService, IPokemonAbilitiesService
    {
        public PokemonAbilitiesService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache,cached)
        {
        }

        //call in pokemonAplication
        public async Task PokemonAbilitiesAddOn(Guid pokeID,PostPokemonAbilitiesDTO model)
        {
            var newPokemonAbilities = _mapper.Map<PokemonAbilities>(model);
            newPokemonAbilities.pokeID = pokeID;
            await _uow.PokemonAbilities.AddAsync(newPokemonAbilities);
        }

        public async Task<bool> DeletePokemonAbilities(Guid pokeID, Guid abID)
        {
            var entry = await _uow.PokemonAbilities.GetLinkAsync(pokeID, abID);
            if (entry == null)
                throw new NotFoundException("Pokemon Don't have this Abilities");

            await _uow.PokemonAbilities.RemoveAsync(entry);
            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonAbilitiesDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetPokemonWithAbilitiesAsync(pokeID);

            if (pokemon == null)
                throw new NotFoundException($"Can't find Pokemons id {pokeID}");

            await _uow.PokemonAbilities.RemoveRange(pokemon.PokemonAbilities);

            var newLinks = _mapper.Map<List<PokemonAbilities>>(model);

            foreach (var link in newLinks)
            {
                link.pokeID = pokeID;
            }

            await _uow.PokemonAbilities.AddRangeAsync(newLinks);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> AddPokemonAbilities(Guid pokeID, PostPokemonAbilitiesDTO model)
        {
            if (!await _uow.Pokemons.ExistsByPokemonByIdAsync(pokeID))
                throw new BadRequestException($"Pokemon Id {pokeID} Not Exist");
            
            if (!await _uow.Abilities.ExistsByIdAsync(model.abID))
                throw new BadRequestException($"Abilities Id {model.abID} Not Exist");
            
            var newPokemonAbilities = _mapper.Map<PokemonAbilities>(model);
            newPokemonAbilities.pokeID = pokeID;

            await _uow.PokemonAbilities.AddAsync(newPokemonAbilities);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }
    }
}
