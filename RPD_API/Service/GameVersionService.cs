using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class GameVersionService : BaseService, IGameVersionService
    {
        public GameVersionService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<GameVersionDTO?> AddGameVersion(PostGameVersionDTO model)
        {
            if (await _uow.GameVersions.ExistsByNameAsync(model.gvName))
                return null;

            var newGameVersion = _mapper.Map<GameVersion>(model);
            await _uow.GameVersions.AddAsync(newGameVersion);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<GameVersionDTO>(newGameVersion) : null;
        }

        public async Task<bool> DeleteGameVersion(Guid gvID)
        {
            var gameVersion = await _uow.GameVersions.GetByIdAsync(gvID);
            if (gameVersion == null)
                return false;

            await _uow.GameVersions.RemoveAsync(gameVersion);

            return await _uow.SaveAsync() > 0;
        }

        public Task<PagedResult<GameVersionDTO>> GetAllGameVersion(QueryParams queryParams)
        {
            return GetPagedAsync<GameVersion, GameVersionDTO>(
                queryParams,
                _uow.GameVersions.GetAllAsync
            );
        }

        public async Task<GameVersionDTO> GetGameVersionById(Guid gvID)
        {
            var gameVersion = await _uow.GameVersions.GetByIdAsync(gvID);

            if (gameVersion == null)
                return null;

            return _mapper.Map<GameVersionDTO>(gameVersion);
        }

        public async Task<bool> UpdateGameVersion(Guid gvID, PutGameVersionDTO model)
        {
            var gameVersion = await _uow.GameVersions.GetByIdAsync(gvID);

            if (gameVersion == null)
                return false;

            if (!string.IsNullOrWhiteSpace(model.gvName))
                gameVersion.gvName = model.gvName;
            if (model.gvGen != 0)
                gameVersion.gvGen = model.gvGen;

            await _uow.GameVersions.UpdateAsync(gameVersion);
            return await _uow.SaveAsync() > 0;
        }
    }
}
