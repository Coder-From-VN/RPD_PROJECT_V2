using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonEggGroupRepo : IBaseRepository
    {
        Task AddAsync(PokemonEggGroup model);
        Task<PokemonEggGroup?> GetLinkAsync(Guid pokeID, Guid egID);
        Task UpdateAsync(PokemonEggGroup model);
        Task RemoveAsync(PokemonEggGroup model);

        Task RemoveRange(IEnumerable<PokemonEggGroup> entities);
        Task AddRangeAsync(List<PokemonEggGroup> entities);
    }
}
