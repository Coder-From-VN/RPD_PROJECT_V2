using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;

namespace RPD_API.Repo.IRepo
{
    public interface IMoveRepo : IBaseRepository
    {
        Task AddAsync(Move model);
        Task<PagedResult<Move>> GetAllAsync(QueryParams query);
        Task<Move?> GetByIdAsync(Guid moveID);
        Task UpdateAsync(Move model);
        Task RemoveAsync(Move model);

        Task<bool> ExistsByNameAsync(string moveName);
    }
}
