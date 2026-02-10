using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;

namespace RPD_API.Repo.IRepo
{
    public interface IGrowthRateRepo : IBaseRepository
    {
        Task AddAsync(GrowthRate model);
        Task<PagedResult<GrowthRate>> GetAllAsync(QueryParams query);
        Task<GrowthRate?> GetByIdAsync(Guid growthRateID);
        Task UpdateAsync(GrowthRate model);
        Task RemoveAsync(GrowthRate model);
        Task AddRangeAsync(List<GrowthRate> abilities);
        Task<List<string>> GetExistingNamesAsync(List<string> names);
        Task<bool> ExistsByNameAsync(string grName);
    }
}
