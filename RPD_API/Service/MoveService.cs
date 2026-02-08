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
    public class MoveService : BaseService, IMoveService
    {
        public MoveService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<MoveDTO?> AddMove(PostMoveDTO model)
        {
            if (await _uow.Moves.ExistsByNameAsync(model.moveName))
                throw new BadRequestException("Move already exists");

            var newMove = _mapper.Map<Move>(model);

            await _uow.Moves.AddAsync(newMove);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<MoveDTO?>(newMove) : null;
        }

        public async Task<bool> DeleteMove(Guid moveID)
        {
            var move = await _uow.Moves.GetByIdAsync(moveID);
            if (move == null)
                throw new NotFoundException($"Moves id {moveID} not found");

            await _uow.Moves.RemoveAsync(move);

            return await _uow.SaveAsync() > 0;

        }

        public async Task<PagedResult<MoveDTO>> GetAllMove(QueryParams queryParams)
        {
            var cacheKey = $"Move:all:page:{queryParams.PageNumber}";

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
                queryParams,
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

        public async Task<bool> UpdateMove(Guid moveID, PutMoveDTO model)
        {
            var move = await _uow.Moves.GetByIdAsync(moveID);
            if (move == null)
                throw new NotFoundException($"Move with id {moveID} not found");

            _mapper.Map(model, move);

            await _uow.Moves.UpdateAsync(move);
            return await _uow.SaveAsync() > 0;

        }
    }
}
