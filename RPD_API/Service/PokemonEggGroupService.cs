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
    public class PokemonEggGroupService : BaseService, IPokemonEggGroupService
    {
        public PokemonEggGroupService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache, cached)
        {
        }

        //call in pokemonAplication
        public async Task PokemonEggGroupAddOn(Guid pokeID,Guid egID)
        {
            PokemonEggGroup newPokemonEggGroup = new PokemonEggGroup
            {
                egID = egID,
                pokeID = pokeID,
            };

            await _uow.PokemonEggGroups.AddAsync(newPokemonEggGroup);
        }

        public async Task<bool> DeletePokemonEggGroup(Guid egID, Guid pokeID)
        {
            var entry = await _uow.PokemonEggGroups.GetLinkAsync(pokeID, egID);
            if (entry == null)
                throw new NotFoundException("Pokemon Don't have this Egg Group");

            await _uow.PokemonEggGroups.RemoveAsync(entry);
            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> UpdatePokemonEggGroup(Guid pokeID, ICollection<PutPokemonEggGroupDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetPokemonWithEggGroupsAsync(pokeID);

            if (pokemon == null)
                throw new NotFoundException($"Can't find Pokemon id {pokeID}");

            await _uow.PokemonEggGroups.RemoveRange(pokemon.PokemonEggGroup);

            var eggGroupIds = model.Select(x => x.egID).ToList();

            var newLinks = _mapper.Map<List<PokemonEggGroup>>(model);

            foreach (var link in newLinks)
            {
                link.pokeID = pokeID;
            }

            await _uow.PokemonEggGroups.AddRangeAsync(newLinks);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> PostPokemonEggGroup(Guid pokeID, Guid egID)
        {
            if (!await _uow.Pokemons.ExistsByPokemonByIdAsync(pokeID))
                throw new BadRequestException($"Pokemon Id {pokeID} Not Exist");

            if (!await _uow.EggGroups.ExistsByIdAsync(egID))
                throw new BadRequestException($"EggGroups Id {egID} Not Exist");


            PokemonEggGroup newPokemonEggGroup = new PokemonEggGroup
            {
                egID = egID,
                pokeID = pokeID,
            };

            await _uow.PokemonEggGroups.AddAsync(newPokemonEggGroup);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }
    }
}
