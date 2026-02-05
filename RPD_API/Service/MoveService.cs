using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.Text.Json;

namespace RPD_API.Service
{
    public class MoveService : BaseService, IMoveService
    {
        public MoveService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<MoveDTO> AddMove(PostMoveDTO model)
        {
            if (await _uow.Moves.ExistsByNameAsync(model.moveName))
                return null;

            var newMove = _mapper.Map<Move>(model);
            await _uow.Moves.AddAsync(newMove);

            if (await _uow.SaveAsync() <= 0)
                return null;

            var moveWithType = await _uow.Moves.GetByIdAsync(newMove.moveID);
            return _mapper.Map<MoveDTO>(moveWithType);
        }

        public async Task<bool> DeleteMove(Guid moveID)
        {
            var move = await _uow.Moves.GetByIdAsync(moveID);
            if (move == null)
                return false;

            await _uow.Moves.RemoveAsync(move);

            return await _uow.SaveAsync() > 0;

        }

        public async Task<PagedResult<MoveDTO>> GetAllMove(QueryParams queryParams)
        {
            var cacheKey = $"Move:all:page:{queryParams.PageNumber}";

            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<PagedResult<MoveDTO>>(cached)!;
            }

            var result = await GetPagedAsync<Move, MoveDTO>(
                queryParams,
                _uow.Moves.GetAllAsync
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

        public async Task<MoveDTO> GetMoveById(Guid moveID)
        {
            var Move = await _uow.Moves.GetByIdAsync(moveID);

            if (Move == null)
                return null;

            return _mapper.Map<MoveDTO>(Move);
        }

        public async Task<bool> UpdateMove(Guid moveID, PutMoveDTO model)
        {
            var move = await _uow.Moves.GetByIdAsync(moveID);
            if (move == null)
                return false;

            _mapper.Map(model, move);

            await _uow.Moves.UpdateAsync(move);
            return await _uow.SaveAsync() > 0;

        }
    }
}
