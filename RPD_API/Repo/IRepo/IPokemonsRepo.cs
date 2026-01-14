using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonsRepo : IBaseRepository
    {
        public Task AddAsync(Pokemons model);
        public Task<List<Pokemons>> GetAllAsync();
        public Task<Pokemons?> GetByIdAsync(Guid pokeID);
        public Task UpdateAsync(Pokemons model);
        public Task RemoveAsync(Pokemons model);

        public Task<bool> ExistsByNationalNumberAsync(int pokeNationalNumber);
    }
}
