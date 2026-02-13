using RPD_API.Models;
using RPD_API.Pagination;
using System.Linq.Expressions;

namespace RPD_API.Repo.IRepo
{
    public interface IAbilitiesRepo : IBaseRepository
    {
        Task AddAsync(Abilities model);
        Task<PagedResult<Abilities>> GetAllAsync(QueryParams query);
        Task<Abilities?> GetByIdAsync(Guid abID);
        Task UpdateAsync(Abilities model);
        Task RemoveAsync(Abilities model);

        Task AddRangeAsync(List<Abilities> abilities);
        Task<List<string>> GetExistingNamesAsync(List<string> names);

        Task<bool> ExistsByNameAsync(string abName);
        Task<bool> ExistsByIdAsync(Guid abID);
    }
}
