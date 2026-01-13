using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IMoveRepo : IBaseRepository
    {
        public Task AddAsync(Move model);
        public Task<List<Move>> GetAllAsync();
        public Task<Move?> GetByIdAsync(Guid moveID);
        public Task UpdateAsync(Move model);
        public Task RemoveAsync(Move model);

        public Task<bool> ExistsByNameAsync(string moveName);
    }
}
