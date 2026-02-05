using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RPD_API.Service
{
    public class PokemonService : BaseService, IPokemonService
    {
        public PokemonService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<PokemonDetailDTO?> GetPokemonsById(Guid pokeID)
        {
            var cacheKey = $"Pokemons:pokeid:{pokeID}";

            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<PokemonDetailDTO>(cached)!; 
            }

            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);

            if (pokemon == null)
                return null;
            var dto = _mapper.Map<PokemonDetailDTO>(pokemon);

            await _cache.SetStringAsync(
        cacheKey,
        JsonSerializer.Serialize(dto), 
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
        });

            return dto;
        }

        public async Task<bool> DeletePokemons(Guid pokeID)
        {
            var Pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (Pokemon == null)
                return false;

            await _uow.Pokemons.RemoveAsync(Pokemon);

            return await _uow.SaveAsync() > 0;
        }

        public async Task<PokemonsDTO?> PostPokemons(PostPokemonDTO model)
        {
            if (await _uow.Pokemons.ExistsByNationalNumberAsync(model.pokeNationalNumber))
                return null;

            var newPokemons = _mapper.Map<Pokemons>(model);
            await _uow.Pokemons.AddAsync(newPokemons);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<PokemonsDTO?>(newPokemons) : null;
        }

        public async Task<bool> PutPokemons(Guid pokeId, PutPokemonDTO model)
        {
            var pokemons = await _uow.Pokemons.GetByIdAsync(pokeId);
            if (pokemons == null)
                return false;

            _mapper.Map(model, pokemons);

            await _uow.Pokemons.UpdateAsync(pokemons);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<PagedResult<PokemonsDTO>> GetAllPokemons(QueryParams query)
        {
            var cacheKey = $"Pokemons:all:page:{query.PageNumber}";

            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<PagedResult<PokemonsDTO>>(cached)!;
            }

            var result = await GetPagedAsync<Pokemons, PokemonsDTO>(
                query,
                _uow.Pokemons.GetAllAsync
            );

            await _cache.SetStringAsync(
          cacheKey,
          JsonSerializer.Serialize(result),
          new DistributedCacheEntryOptions
          {
              AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
          });

            return result;
        }
    }
}
