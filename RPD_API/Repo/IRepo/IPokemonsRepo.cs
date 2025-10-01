using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonsRepo
    {
        public Task<List<PokemonsDTO>> GetAllPokemons();
        public Task<PokemonsDTO> GetPokemonsById(Guid pokeID);
        public Task<PokemonsDTO> AddPokemons(PostPokemonDTO model);
        public Task<bool> UpdatePokemons(Guid pokeID, PokemonsDTO model);
        public Task<bool> DeletePokemons(Guid pokeID);
    }
}
