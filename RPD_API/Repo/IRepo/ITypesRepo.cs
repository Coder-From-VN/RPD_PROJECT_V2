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

        public Task<bool> ExistsByNameAsync(string typesName);
    }
}
