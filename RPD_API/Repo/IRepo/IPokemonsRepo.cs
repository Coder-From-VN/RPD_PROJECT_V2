using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonsRepo
    {
        //nedd fix
        public Task<List<PokemonsDTO>> GetAllPokemons();
        //nedd fix
        public Task<PokemonsDTO> GetPokemonsById(Guid pokeID);
        //on going
        public Task<PokemonsDTO> AddPokemons(PostPokemonDTO model);
        //nedd fix
        public Task<bool> UpdatePokemons(Guid pokeID, PokemonsDTO model);
        //nedd fix
        public Task<bool> DeletePokemons(Guid pokeID);
    }
}
