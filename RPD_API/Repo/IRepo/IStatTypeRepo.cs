using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IStatTypeRepo : IBaseRepository
    {
        public Task AddAsync(StatType model);
        public Task<List<StatType>> GetAllAsync();
        public Task<StatType?> GetByIdAsync(Guid stID);
        public Task UpdateAsync(StatType model);
        public Task RemoveAsync(StatType model);
        Task AddRangeAsync(List<StatType> stList);

        Task<List<string>> GetExistingNamesAsync(List<string> names);
        public Task<bool> ExistsByNameAsync(string stName);
        public Task<bool> ExistsByIdAsync(Guid stID);
    }
}
