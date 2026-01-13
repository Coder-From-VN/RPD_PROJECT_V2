using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IGameVersionService : IBaseService
    {
        public Task<List<GameVersionDTO>> GetAllGameVersion();
        public Task<GameVersionDTO> GetGameVersionById(Guid gvID);
        public Task<GameVersionDTO?> AddGameVersion(PostGameVersionDTO model);
        public Task<bool> UpdateGameVersion(Guid gvID, PutGameVersionDTO model);
        public Task<bool> DeleteGameVersion(Guid gvID);
    }
}
