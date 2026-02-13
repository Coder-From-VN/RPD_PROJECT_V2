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
    public class GameVersionService : BaseService, IGameVersionService
    {
        public GameVersionService(
            IUnitOfWorkRepo uow, 
            IMapper mapper, 
            IDistributedCache cache,
            ICacheService cached)
        : base(uow, mapper, cache,cached)
        {
        }

        public async Task<GameVersionDTO?> AddGameVersion(PostGameVersionDTO model)
        {
            if (await _uow.GameVersions.ExistsByNameAsync(model.gvName))
                throw new BadRequestException("GameVersions name already exists");

            var newGameVersion = _mapper.Map<GameVersion>(model);
            await _uow.GameVersions.AddAsync(newGameVersion);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"GameVersions:all:");
            }
            else
            {
                throw new BadRequestException("GameVersion Post Fail");
            }

            return _mapper.Map<GameVersionDTO>(newGameVersion);
        }

        public async Task<bool> DeleteGameVersion(Guid gvID)
        {
            var gameVersion = await _uow.GameVersions.GetByIdAsync(gvID);
            if (gameVersion == null)
                throw new NotFoundException($"GameVersions id {gvID} not found");

            await _uow.GameVersions.RemoveAsync(gameVersion);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"GameVersions:all:");
            }

            return saved;
        }

        public async Task<PagedResult<GameVersionDTO>> GetAllGameVersion(QueryParams queryParams)
        {
            var cacheKey = $"GameVersions:all:page:{queryParams.PageNumber}:size:{queryParams.PageSize}";
            try
            {
                var cached = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cached))
                {
                    return JsonSerializer.Deserialize<PagedResult<GameVersionDTO>>(cached)!;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"cache conect Fail {ex}");

            }
            var result = await GetPagedAsync<GameVersion, GameVersionDTO>(
                queryParams,
                _uow.GameVersions.GetAllAsync
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

        public async Task<GameVersionDTO> GetGameVersionById(Guid gvID)
        {
            var gameVersion = await _uow.GameVersions.GetByIdAsync(gvID);

            if (gameVersion == null)
                throw new NotFoundException($"GameVersions with id {gvID} not found");

            return _mapper.Map<GameVersionDTO>(gameVersion);
        }

        public async Task<int> ImportGameVersionAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var gvDtos = csv.GetRecords<PostGameVersionDTO>().ToList();

            var normalizedDtos = gvDtos
                .Where(x => !string.IsNullOrWhiteSpace(x.gvName))
                .GroupBy(x => x.gvName.Trim().ToLower())
                .Select(g => g.First())
                .ToList();

            var names = normalizedDtos
                .Select(x => x.gvName)
                .ToList();

            var existingNames = await _uow.GameVersions
                .GetExistingNamesAsync(names);

            var newDtos = normalizedDtos
                .Where(x => !existingNames.Contains(x.gvName))
                .ToList();

            var gameVersions = _mapper.Map<List<GameVersion>>(newDtos);

            if (!gameVersions.Any())
                return 0;

            await _uow.GameVersions.AddRangeAsync(gameVersions);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"GameVersions:all:");
            }
            else
            {
                throw new BadRequestException("something worng with abilities list");
            }

            return gameVersions.Count;
        }

        public async Task<bool> UpdateGameVersion(Guid gvID, PutGameVersionDTO model)
        {
            var gameVersion = await _uow.GameVersions.GetByIdAsync(gvID);

            if (gameVersion == null)
                throw new NotFoundException($"GameVersions id {gvID} not found");

            _mapper.Map(model, gameVersion);

            await _uow.GameVersions.UpdateAsync(gameVersion);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"GameVersions:all:");
            }

            return saved;
        }
    }
}
