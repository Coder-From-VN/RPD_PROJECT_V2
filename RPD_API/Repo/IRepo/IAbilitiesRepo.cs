using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IAbilitiesRepo : IBaseRepository
    {
        public Task AddAsync(Abilities model);
        public Task<List<Abilities>> GetAllAsync();
        public Task<Abilities?> GetByIdAsync(Guid abID);
        public Task UpdateAsync(Abilities model);
        public Task RemoveAsync(Abilities model);

        public Task<bool> ExistsByNameAsync(string abName);
    }
}
