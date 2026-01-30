using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;

namespace RPD_API.Repo.IRepo
{
    public interface IAbilitiesRepo : IBaseRepository
    {
        Task AddAsync(Abilities model);
        Task<PagedResult<Abilities>> GetAllAsync(QueryParams query);
        Task<Abilities?> GetByIdAsync(Guid abID);
        Task UpdateAsync(Abilities model);
        Task RemoveAsync(Abilities model);

        Task<bool> ExistsByNameAsync(string abName);
    }
}
