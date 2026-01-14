using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IPokemonService : IBaseService
    {
        public Task<List<PokemonsDTO>> GetAllPokemons();
        public Task<PokemonDetailDTO> GetPokemonsById(Guid pokeID);
        public Task<PokemonsDTO?> PostPokemons(PostPokemonDTO model);
        public Task<bool> PutPokemons(Guid pokeId, PutPokemonDTO model);
        public Task<bool> DeletePokemons(Guid pokeID);
    }
}
