using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IGameVersionRepo : IBaseRepository
    {
        public Task AddAsync(GameVersion model);
        public Task<List<GameVersion>> GetAllAsync();
        public Task<GameVersion?> GetByIdAsync(Guid gvID);
        public Task UpdateAsync(GameVersion model);
        public Task RemoveAsync(GameVersion model);

        public Task<bool> ExistsByNameAsync(string gvName);
    }
}
