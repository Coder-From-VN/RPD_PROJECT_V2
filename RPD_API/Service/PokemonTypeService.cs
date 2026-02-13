using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.DTO;
using RPD_API.DTO.Types;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonTypeService : BaseService, IPokemonTypeService
    {
        public PokemonTypeService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache,cached)
        {
        }

        //call in pokemonAplication
        public async Task PokemonTypeAddOn(Guid pokeID, PostPokemonTypeDTO model)
        {
            var newPokemonType = _mapper.Map<PokemonType>(model);

            newPokemonType.pokeID = pokeID;

            await _uow.PokemonTypes.AddAsync(newPokemonType);
        }

        public async Task<bool> DeletePokemonType(Guid pokeID,Guid typesID)
        {
            var entry = await _uow.PokemonTypes.GetLinkAsync(pokeID, typesID);
            if (entry == null)
                throw new NotFoundException("Pokemon Don't Have This Type");

            await _uow.PokemonTypes.RemoveAsync(entry);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> UpdatePokemonType(Guid pokeID, ICollection<PutPokemonTypeDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetPokemonWithTypesAsync(pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Pokemons with ID {pokeID} Not Found");

            if (pokemon.PokemonType != null && pokemon.PokemonType.Any())
            {
                await _uow.PokemonTypes.RemoveRange(pokemon.PokemonType);
            }

            var newLinks = _mapper.Map<List<PokemonType>>(model);

            newLinks.ForEach(x => x.pokeID = pokeID);

            await _uow.PokemonTypes.AddRangeAsync(newLinks);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> PostPokemonType(Guid pokeID, PostPokemonTypeDTO model)
        {
            if (!await _uow.Pokemons.ExistsByPokemonByIdAsync(pokeID))
                throw new BadRequestException($"Pokemon Id {pokeID} Not Exist");

            if (!await _uow.Types.ExistsByIdAsync(model.typesID))
                throw new BadRequestException($"Abilities Id {model.typesID} Not Exist");

            var newPokemonType = _mapper.Map<PokemonType>(model);

            newPokemonType.pokeID = pokeID;

            await _uow.PokemonTypes.AddAsync(newPokemonType);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }
    }
}
