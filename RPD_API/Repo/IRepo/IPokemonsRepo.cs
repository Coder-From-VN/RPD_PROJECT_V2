using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonsRepo : IBaseRepository
    {
        Task AddAsync(Pokemons model);
        Task<PagedResult<Pokemons>> GetAllAsync(QueryParams query);
        Task<Pokemons?> GetByIdAsync(Guid pokeID);
        Task UpdateAsync(Pokemons model);
        Task RemoveAsync(Pokemons model);

        Task<bool> ExistsByNationalNumberAsync(int pokeNationalNumber);
    }
}
