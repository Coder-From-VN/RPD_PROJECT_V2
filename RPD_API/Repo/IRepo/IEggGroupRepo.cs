using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IEggGroupRepo : IBaseRepository
    {
        public Task AddAsync(EggGroup model);
        public Task<List<EggGroup>> GetAllAsync();
        public Task<EggGroup?> GetByIdAsync(Guid egID);
        public Task UpdateAsync(EggGroup model);
        public Task RemoveAsync(EggGroup model);

        public Task<bool> ExistsByNameAsync(string egName);

    }
}
