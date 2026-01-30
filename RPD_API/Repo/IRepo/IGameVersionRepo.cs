using RPD_API.Models;
using RPD_API.Pagination;

namespace RPD_API.Repo.IRepo
{
    public interface IGameVersionRepo : IBaseRepository
    {
        Task AddAsync(GameVersion model);
        Task<PagedResult<GameVersion>> GetAllAsync(QueryParams query);
        Task<GameVersion?> GetByIdAsync(Guid gvID);
        Task UpdateAsync(GameVersion model);
        Task RemoveAsync(GameVersion model);

        Task<bool> ExistsByNameAsync(string gvName);
    }
}
