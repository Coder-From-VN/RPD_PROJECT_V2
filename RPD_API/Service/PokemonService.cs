using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using Serilog;
using System.Text.Json;

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

            try
            {
                var cached = await _cache.GetStringAsync(cacheKey);

                if (cached != null)
                {
                    return JsonSerializer.Deserialize<PokemonDetailDTO>(cached)!;
                }
            }
            catch (Exception ex) { Log.Error($"cache read Fail {ex}"); }

            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);

            if (pokemon == null)
                throw new NotFoundException($"Pokemons with id {pokeID} not found");

            var result = _mapper.Map<PokemonDetailDTO>(pokemon);

            try
            {
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
                    });
            }
            catch (Exception ex)
            {
                Log.Error($"cache write Fail {ex}");
            }
            return result;
        }

        public async Task<bool> DeletePokemons(Guid pokeID)
        {
            var Pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (Pokemon == null)
                throw new NotFoundException($"Pokemons with id {pokeID} not found");

            await _uow.Pokemons.RemoveAsync(Pokemon);

            return await _uow.SaveAsync() > 0;
        }

        public async Task<PokemonsDTO?> PostPokemons(PostPokemonDTO model)
        {
            if (await _uow.Pokemons.ExistsByNationalNumberAsync(model.pokeNationalNumber))
                throw new BadRequestException($"Pokemons with NationalNumber {model.pokeNationalNumber} Exits");

            var newPokemons = _mapper.Map<Pokemons>(model);
            await _uow.Pokemons.AddAsync(newPokemons);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<PokemonsDTO?>(newPokemons) : throw new BadRequestException("Something wrong when add new Pokemon");
        }

        public async Task<bool> PutPokemons(Guid pokeId, PutPokemonDTO model)
        {
            var pokemons = await _uow.Pokemons.GetByIdAsync(pokeId);
            if (pokemons == null)
                throw new NotFoundException($"Pokemons with id {pokeId} not found");

            _mapper.Map(model, pokemons);

            await _uow.Pokemons.UpdateAsync(pokemons);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<PagedResult<PokemonsDTO>> GetAllPokemons(QueryParams query)
        {
            var cacheKey = $"Pokemons:all:page:{query.PageNumber}";

            try
            {
                var cached = await _cache.GetStringAsync(cacheKey);

                if (cached != null)
                {
                    return JsonSerializer.Deserialize<PagedResult<PokemonsDTO>>(cached)!;
                }
            }
            catch (Exception ex) { Log.Error($"cache read Fail {ex}"); }

            var result = await GetPagedAsync<Pokemons, PokemonsDTO>(
                query,
                _uow.Pokemons.GetAllAsync
            );

            try
            {
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(result),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
                    });
            }
            catch (Exception ex)
            {
                Log.Error($"cache write Fail {ex}");
            }
            return result;
        }

    }
}
