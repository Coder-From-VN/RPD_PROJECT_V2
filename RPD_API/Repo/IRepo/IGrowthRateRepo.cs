using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IGrowthRateRepo : IBaseRepository
    {
        public Task AddAsync(GrowthRate model);
        public Task<List<GrowthRate>> GetAllAsync();
        public Task<GrowthRate?> GetByIdAsync(Guid growthRateID);
        public Task UpdateAsync(GrowthRate model);
        public Task RemoveAsync(GrowthRate model);

        public Task<bool> ExistsByNameAsync(string grName);
    }
}
