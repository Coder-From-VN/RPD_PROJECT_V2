using AutoMapper;
using CsvHelper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using Serilog;
using System.Globalization;
using System.Text.Json;

namespace RPD_API.Service
{
    public class PokemonService : BaseService, IPokemonService
    {
        public PokemonService(
            IUnitOfWorkRepo uow, 
            IMapper mapper, 
            IDistributedCache cache, 
            ICacheService cached)
        : base(uow, mapper, cache, cached)
        {
        }

        public async Task<PokemonDetailDTO> GetPokemonsById(Guid pokeID)
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

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
                await _cached.RemoveByPrefixAsync($"Pokemons:all:page:");
            }

            return saved;
        }

        public async Task<PokemonsDTO?> PostPokemons(PostPokemonDTO model)
        {
            var newPokemons = _mapper.Map<Pokemons>(model);
            await _uow.Pokemons.AddAsync(newPokemons);

            return _mapper.Map<PokemonsDTO?>(newPokemons);
        }
        //check at pokeaplication
        public async Task PutPokemons(Guid pokeId, PutPokemonDTO model)
        {
            var pokemons = await _uow.Pokemons.GetByIdAsync(pokeId);
            if (pokemons == null)
                throw new NotFoundException($"Pokemons with id {pokeId} not found");

            _mapper.Map(model, pokemons);

            await _uow.Pokemons.UpdateAsync(pokemons);
        }

        public async Task<PagedResult<PokemonsDTO>> GetAllPokemons(QueryParams query)
        {
            var cacheKey = $"Pokemons:all:page:{query.PageNumber}:size:{query.PageSize}";
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
        //call at pokeapplication
        public async Task<int> ImportPokemonsAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var pokemonDtos = csv.GetRecords<PostPokemonDTO>().ToList();

            var normalizedDtos = pokemonDtos
                .Where(x => !string.IsNullOrWhiteSpace(x.pokeName))
                .GroupBy(x => x.pokeNationalNumber)
                .Select(g => g.First())
                .ToList();

            var nationalNumbers = normalizedDtos
                .Select(x => x.pokeNationalNumber)
                .ToList();

            var existingNationalNumbers = await _uow.Pokemons
                .GetExistingpokeNationalNumberAsync(nationalNumbers);

            var newDtos = normalizedDtos
                .Where(x => !existingNationalNumbers.Contains(x.pokeNationalNumber))
                .ToList();

            if (!newDtos.Any())
                return 0;

            var pokemons = _mapper.Map<List<Pokemons>>(newDtos);

            await _uow.Pokemons.AddRangeAsync(pokemons);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"Pokemons:all:page:");
            }
            else
            {
                throw new BadRequestException("something worng with abilities list");
            }

            return pokemons.Count;

        }
    }
}
