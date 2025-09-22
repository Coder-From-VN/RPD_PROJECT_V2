using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IGameVersionRepo
    {
        public Task<List<GameVersionDTO>> GetAllGameVersion();
        public Task<GameVersionDTO> GetGameVersionById(Guid gvID);
        public Task<Guid> AddGameVersion(GameVersionDTO model);
        public Task UpdateGameVersion(Guid gvID, GameVersionDTO model);
        public Task DeleteGameVersion(Guid gvID);
    }
}
