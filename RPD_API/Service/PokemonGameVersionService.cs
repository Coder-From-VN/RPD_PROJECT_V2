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
    public class PokemonGameVersionService : BaseService, IPokemonGameVersionService
    {
        public PokemonGameVersionService(
            IUnitOfWorkRepo uow, 
            IMapper mapper, 
            IDistributedCache cache,
            ICacheService cached
            )
        : base(uow, mapper, cache,cached)
        {
        }
        //call in PokemonApplication
        public async Task PokemonGameVersionAddOn(Guid pokeID, PostPokemonGameVersionDTO model)
        {
            var newPokemonGameVersion = _mapper.Map<PokemonGameVersion>(model);

            newPokemonGameVersion.pokeID = pokeID;

            await _uow.PokemonGameVersions.AddAsync(newPokemonGameVersion);
        }

        public async Task<bool> DeletePokemonGameVersion(Guid pokeID, Guid gvID)
        {
            var entry = await _uow.PokemonGameVersions.GetLinkAsync(pokeID, gvID);
            if (entry == null)
                throw new NotFoundException("GameVersion Not Add To Pokemon Yet");

            await _uow.PokemonGameVersions.RemoveAsync(entry);
            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> UpdatePokemonGameVersion(Guid pokeID, ICollection<PutPokemonGameVersionDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetPokemonWithGameVersionAsync(pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Can't find Pokemons id {pokeID}");

            await _uow.PokemonGameVersions.RemoveRange(pokemon.PokemonGameVersion);

            var newLinks = _mapper.Map<List<PokemonGameVersion>>(model);

            foreach (var link in newLinks)
            {
                link.pokeID = pokeID;
            }

            await _uow.PokemonGameVersions.AddRangeAsync(newLinks);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> PostPokemonGameVersion(Guid pokeID, PostPokemonGameVersionDTO model)
        {
            if (!await _uow.Pokemons.ExistsByPokemonByIdAsync(pokeID))
                throw new BadRequestException($"Pokemon Id {pokeID} Not Exist");

            if (!await _uow.GameVersions.ExistsByIdAsync(model.gvID))
                throw new BadRequestException($"GameVersions Id {model.gvID} Not Exist");

            var newPokemonGameVersion = _mapper.Map<PokemonGameVersion>(model);
            newPokemonGameVersion.pokeID = pokeID;

            await _uow.PokemonGameVersions.AddAsync(newPokemonGameVersion);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }
    }
}
