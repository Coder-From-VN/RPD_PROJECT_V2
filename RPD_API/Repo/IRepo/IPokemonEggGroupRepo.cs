using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonEggGroupRepo : IBaseRepository
    {
        public Task AddAsync(PokemonEggGroup model);
        public Task<PokemonEggGroup?> GetLinkAsync(Guid pokeID, Guid egID);
        public Task UpdateAsync(PokemonEggGroup model);
        public Task RemoveAsync(PokemonEggGroup model);
    }
}
