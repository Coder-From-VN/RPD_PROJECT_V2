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
    public class PokemonStatsService : BaseService, IPokemonStatsService
    {
        public PokemonStatsService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache,cached)
        {
        }

        //call in pokemonAplication
        public async Task PokemonStatsAddOn(Guid pokeID, PostPokemonStatsDTO model)
        {
            var newPokemonStats = _mapper.Map<PokemonStats>(model);

            newPokemonStats.pokeID = pokeID;

            await _uow.PokemonStats.AddAsync(newPokemonStats);
           
        }

        public async Task<bool> AddPokemonStats(Guid pokeID, PostPokemonStatsDTO model)
        {
            if (!await _uow.Pokemons.ExistsByPokemonByIdAsync(pokeID))
                throw new BadRequestException($"Pokemon Id {pokeID} Not Exist");

            if (!await _uow.StatTypes.ExistsByIdAsync(model.stID))
                throw new BadRequestException($"StatTypes Id {model.stID} Not Exist");

            var newPokemonStats = _mapper.Map<PokemonStats>(model);
            newPokemonStats.pokeID = pokeID;

            await _uow.PokemonStats.AddAsync(newPokemonStats);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> DeletePokemonStats(Guid pokeID, Guid stID)
        {
            var entry = await _uow.PokemonStats.GetLinkAsync(pokeID, stID);
            if (entry == null)
                throw new NotFoundException($"Can't Find Stat id {stID} in Pokemon id {pokeID}");

            await _uow.PokemonStats.RemoveAsync(entry);
            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<bool> UpdatePokemonStats(Guid pokeID, ICollection<PutPokemonStatsDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetPokemonWithAbilitiesAsync(pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Can't Find Pokemon id {pokeID}");

            await _uow.PokemonStats.RemoveRange(pokemon.PokemonStats);

            var newLinks = _mapper.Map<List<PokemonStats>>(model);

            foreach (var link in newLinks)
            {
                link.pokeID = pokeID;
            }

            await _uow.PokemonStats.AddRangeAsync(newLinks);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }
    }
}
