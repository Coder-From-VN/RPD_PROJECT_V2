using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface ITypesRepo : IBaseRepository
    {
        public Task AddAsync(Types model);
        public Task<List<Types>> GetAllAsync();
        public Task<Types?> GetByIdAsync(Guid typesID);
        public Task UpdateAsync(Types model);
        public Task RemoveAsync(Types model);
        Task AddRangeAsync(List<Types> typesList);
        Task<List<string>> GetExistingNamesAsync(List<string> names);
        public Task<bool> ExistsByNameAsync(string typesName);
        public Task<bool> ExistsByIdAsync(Guid typesID);
    }
}
