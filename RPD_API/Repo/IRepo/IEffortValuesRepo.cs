using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IEffortValuesRepo : IBaseRepository
    {
        public Task AddAsync(EffortValues model);
        public Task<List<EffortValues>> GetAllAsync();
        public Task<EffortValues?> GetByIdAsync(Guid abID);
        public Task UpdateAsync(EffortValues model);
        public Task RemoveAsync(EffortValues model);
    }
}
