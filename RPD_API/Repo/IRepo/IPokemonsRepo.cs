using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonsRepo : IBaseRepository
    {
        public Task<List<PokemonsDTO>> GetAllPokemons();
        public Task<PokemonDetailDTO> GetPokemonsById(Guid pokeID);
        public Task AddPokemons(Pokemons model);
        public Task<bool> UpdatePokemons(Guid pokeID, PutPokemonDTO model);
        public Task<bool> DeletePokemons(Guid pokeID);
        public Task<Pokemons> FindPokemonsById(Guid pokeID);
        public Task<bool> CheckPokemonExited(int pokeNationalNumber);
        public Task<Pokemons?> GetPokemonWithEVsAsync(Guid pokeID);
    }
}
