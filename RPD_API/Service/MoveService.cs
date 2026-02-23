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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace RPD_API.Service
{
    public class MoveService : BaseService, IMoveService
    {
        public MoveService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache, cached)
        {
        }

        public async Task<MoveDTO> AddMove(PostMoveDTO model)
        {
            if (await _uow.Moves.ExistsByNameAsync(model.moveName))
                throw new BadRequestException("Move already exists");

            var newMove = _mapper.Map<Move>(model);

            await _uow.Moves.AddAsync(newMove);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"Moves:all:page:");
            }
            else
            {
                throw new BadRequestException("Move ADD Fail");
            }

            return _mapper.Map<MoveDTO>(newMove);
        }

        public async Task<bool> DeleteMove(Guid moveID)
        {
            var move = await _uow.Moves.GetByIdAsync(moveID);
            if (move == null)
                throw new NotFoundException($"Moves id {moveID} not found");

            await _uow.Moves.RemoveAsync(move);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"Moves:all:page:");
            }

            return saved;
        }

        public async Task<PagedResult<MoveDTO>> GetAllMove(QueryParams query)
        {
            var cacheKey = $"Moves:all:PageNumber:{query.PageNumber}" +
                            $":PageSize:{query.PageSize}" +
                            $":Search:{query.Search}" +
                            $":SortBy:{query.SortBy}" +
                            $":SortOrder:{query.SortOrder}";

            try
            {
                var cached = await _cache.GetStringAsync(cacheKey);

                if (cached != null)
                {
                    return JsonSerializer.Deserialize<PagedResult<MoveDTO>>(cached)!;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"cache read Fail {ex}");
            }

            var result = await GetPagedAsync<Move, MoveDTO>(
                query,
                _uow.Moves.GetAllAsync
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

        public async Task<MoveDTO> GetMoveById(Guid moveID)
        {
            var Move = await _uow.Moves.GetByIdAsync(moveID);

            if (Move == null)
                throw new NotFoundException($"Move with id {moveID} not found");

            return _mapper.Map<MoveDTO>(Move);
        }

        public async Task<int> ImportMoveAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var mDtos = csv.GetRecords<PostMoveDTO>().ToList();

            if (!mDtos.Any())
                return 0;

            var normalizedDtos = mDtos
                .Where(x => !string.IsNullOrWhiteSpace(x.moveName))
                .GroupBy(x => x.moveName.ToLower())
                .Select(g => g.First())
                .ToList();

            if (!normalizedDtos.Any())
                return 0;

            var names = normalizedDtos
                .Select(x => x.moveName)
                .ToList();

            var existingNames = await _uow.Moves
                .GetExistingNamesAsync(names);

            var newDtos = normalizedDtos
                .Where(x => !existingNames.Contains(x.moveName))
                .ToList();

            var moves= _mapper.Map<List<Move>>(newDtos);

            if (!moves.Any())
                return 0;

            await _uow.Moves.AddRangeAsync(moves);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"Moves:all:page:");
            }
            else
            {
                throw new BadRequestException("something worng with abilities list");
            }

            return moves.Count;
        }

        public async Task<bool> UpdateMove(Guid moveID, PutMoveDTO model)
        {
            var move = await _uow.Moves.GetByIdAsync(moveID);
            if (move == null)
                throw new NotFoundException($"Move with id {moveID} not found");

            _mapper.Map(model, move);

            await _uow.Moves.UpdateAsync(move);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"Moves:all:page:");
            }

            return saved;

        }
    }
}
