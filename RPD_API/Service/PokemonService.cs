using AutoMapper;
using CsvHelper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.DTO;
using RPD_API.DTO.Pokemon;
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

            return await GetOrSetCacheAsync(
                cacheKey,
                async () =>
                {
                    var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);

                    if (pokemon == null)
                        throw new NotFoundException($"Pokemons with id {pokeID} not found");

                    return _mapper.Map<PokemonDetailDTO>(pokemon);
                },
                TimeSpan.FromMinutes(3)
            );
        }

        public async Task<bool> DeletePokemons(Guid pokeID)
        {
            await _uow.EvolutionCharts.RemoveRangeAsync(pokeID);
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
            var cacheKey = $"Pokemons:all:PageNumber:{query.PageNumber}" +
                            $":PageSize:{query.PageSize}" +
                            $":Search:{query.Search}" +
                            $":SortBy:{query.SortBy}" +
                            $":SortOrder:{query.SortOrder}";

            return await GetOrSetCacheAsync(
                cacheKey,
                () => GetPagedAsync<Pokemons, PokemonsDTO>(query, _uow.Pokemons.GetAllAsync),
                TimeSpan.FromMinutes(3)
            );
        }

        //call at pokeapplication
        public async Task<int> ImportPokemonsAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<PokemonMegaCsvDTO>().ToList();
            if (!records.Any())
                return 0;

            var nationalNumbers = records.Select(r => r.pokeNationalNumber).ToList();
            var existing = await _uow.Pokemons
                .GetExistingpokeNationalNumberAsync(nationalNumbers);

            var existingSet = existing.ToHashSet();

            var newRecords = records
                .Where(r => !existingSet.Contains(r.pokeNationalNumber))
                .ToList();

            if (!newRecords.Any())
                return 0;

            var pokemons = newRecords.Select(r =>
            {
                var entity = _mapper.Map<Pokemons>(r);

                entity.ImageLink = ParseImages(r.ImageLinks);
                entity.EffortValues = ParseEffort(r.EffortValues);
                entity.PokemonStats = ParseStats(r.PokemonStats);
                entity.PokemonAbilities = ParseAbilities(r.PokemonAbilities);
                entity.PokemonGameVersion = ParseGameVersions(r.PokemonGameVersion);
                entity.PokemonEggGroup = ParseEggGroups(r.PokemonEggGroup);
                entity.PokemonType = ParseTypes(r.PokemonType);

                return entity;
            }).ToList();

            await _uow.Pokemons.AddRangeAsync(pokemons);
            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"Pokemons:all:page:");
            }

            return pokemons.Count;

        }


        private ICollection<ImageLink> ParseImages(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<ImageLink>();

            return input.Split('|')
                .Select(x => new ImageLink
                {
                    imgLink = x.Trim()
                })
                .ToList();
        }

        private ICollection<EffortValues> ParseEffort(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<EffortValues>();

            return input.Split('|')
                .Select(x =>
                {
                    var parts = x.Split(':');
                    if (parts.Length != 2) return null;

                    return new EffortValues
                    {
                        evStatName = parts[0],
                        eValues = int.TryParse(parts[1], out var val) ? val : 0
                    };
                })
                .Where(x => x != null)
                .ToList();
        }

        private ICollection<PokemonStats> ParseStats(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<PokemonStats>();

            return input.Split('|')
                .Select(x =>
                {
                    var parts = x.Split(':');
                    if (parts.Length != 4) return null;

                    return new PokemonStats
                    {
                        stID = Guid.TryParse(parts[0], out var id) ? id : Guid.Empty,
                        Basevalue = int.TryParse(parts[1], out var baseVal) ? baseVal : 0,
                        minValue = int.TryParse(parts[2], out var min) ? min : 0,
                        MaxValue = int.TryParse(parts[3], out var max) ? max : 0
                    };
                })
                .Where(x => x != null && x.stID != Guid.Empty)
                .ToList();
        }

        private ICollection<PokemonAbilities> ParseAbilities(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<PokemonAbilities>();

            return input.Split('|')
                .Select(x =>
                {
                    var parts = x.Split(':');
                    if (parts.Length != 2) return null;

                    return new PokemonAbilities
                    {
                        abID = Guid.TryParse(parts[0], out var id) ? id : Guid.Empty,
                        paHiddenCheck = bool.TryParse(parts[1], out var hidden) && hidden
                    };
                })
                .Where(x => x != null && x.abID != Guid.Empty)
                .ToList();
        }

        private ICollection<PokemonGameVersion> ParseGameVersions(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<PokemonGameVersion>();

            return input.Split('|')
                .Select(x =>
                {
                    var parts = x.Split(':');
                    if (parts.Length < 3) return null;

                    return new PokemonGameVersion
                    {
                        gvID = Guid.TryParse(parts[0], out var id) ? id : Guid.Empty,
                        pgvDexNumber = int.TryParse(parts[1], out var dex) ? dex : 0,
                        pgvEntries = parts[2]
                    };
                })
                .Where(x => x != null && x.gvID != Guid.Empty)
                .ToList();
        }

        private ICollection<PokemonEggGroup> ParseEggGroups(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<PokemonEggGroup>();

            return input.Split('|')
                .Select(x =>
                {
                    return new PokemonEggGroup
                    {
                        egID = Guid.TryParse(x, out var id) ? id : Guid.Empty
                    };
                })
                .Where(x => x.egID != Guid.Empty)
                .ToList();
        }

        private ICollection<PokemonType> ParseTypes(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return new List<PokemonType>();

            return input.Split('|')
                .Select(x =>
                {
                    var parts = x.Split(':');
                    if (parts.Length != 2) return null;

                    return new PokemonType
                    {
                        typesID = Guid.TryParse(parts[0], out var id) ? id : Guid.Empty,
                        MainOrSubType = int.TryParse(parts[1], out var type) ? type : 0
                    };
                })
                .Where(x => x != null && x.typesID != Guid.Empty)
                .ToList();
        }
    }
}
