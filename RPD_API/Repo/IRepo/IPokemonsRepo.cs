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
        Task<bool> ExistsByPokemonByIdAsync(Guid pokeID);
        Task<List<int>> GetExistingpokeNationalNumberAsync(List<int> pokeNationalNumber);
        Task AddRangeAsync(List<Pokemons> pokeList);

        //pokemonAbilities
        Task<Pokemons?> GetPokemonWithAbilitiesAsync(Guid pokeID);
        //pokemonEggGroup
        Task<Pokemons?> GetPokemonWithEggGroupsAsync(Guid pokeID);
        //pokemonEggGroup
        Task<Pokemons?> GetPokemonWithGameVersionAsync(Guid pokeID);
        //pokemonStats
        Task<Pokemons?> GetPokemonWithStatsAsync(Guid pokeID);
        //pokemonTypes
        Task<Pokemons?> GetPokemonWithTypesAsync(Guid pokeID);
    }
}
