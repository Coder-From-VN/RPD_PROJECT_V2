using RPD_API.DTO;
using RPD_API.Pagination;

namespace RPD_API.Service.IService
{
    public interface IGameVersionService : IBaseService
    {
        Task<PagedResult<GameVersionDTO>> GetAllGameVersion(QueryParams query);
        Task<GameVersionDTO> GetGameVersionById(Guid gvID);
        Task<GameVersionDTO?> AddGameVersion(PostGameVersionDTO model);
        Task<bool> UpdateGameVersion(Guid gvID, PutGameVersionDTO model);
        Task<bool> DeleteGameVersion(Guid gvID);
    }
}
