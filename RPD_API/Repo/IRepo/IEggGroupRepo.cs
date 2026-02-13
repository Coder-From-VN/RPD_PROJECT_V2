using RPD_API.Models;
using RPD_API.Pagination;

namespace RPD_API.Repo.IRepo
{
    public interface IEggGroupRepo : IBaseRepository
    {
        Task AddAsync(EggGroup model);
        Task AddRangeAsync(List<EggGroup> abilities);
        Task<PagedResult<EggGroup>> GetAllAsync(QueryParams queryParams);
        Task<EggGroup?> GetByIdAsync(Guid egID);
        Task UpdateAsync(EggGroup model);
        Task RemoveAsync(EggGroup model);

        Task<List<string>> GetExistingNamesAsync(List<string> names);
        Task<bool> ExistsByNameAsync(string egName);
        Task<bool> ExistsByIdAsync(Guid egID);
    }
}
