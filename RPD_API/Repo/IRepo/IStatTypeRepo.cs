using RPD_API.DTO;
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

        public Task<bool> ExistsByNameAsync(string stName);
    }
}
