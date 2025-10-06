using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonsRepo
    {
        public Task<List<PokemonsDTO>> GetAllPokemons();
        public Task<PokemonsDTO> GetPokemonsById(Guid pokeID);
        public Task<PokemonsDTO> AddPokemons(PostPokemonDTO model);
        public Task<bool> UpdatePokemons(Guid pokeID, PokemonsDTO model);
        public Task<bool> DeletePokemons(Guid pokeID);
        public Task<Pokemons> FindPokemonsById(Guid pokeID);
    }
}
