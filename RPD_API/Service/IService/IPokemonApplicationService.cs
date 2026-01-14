using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IPokemonApplicationService : IBaseService
    {
        public Task<PokemonDetailDTO?> PostPokemons(PostFullPokemonsDTO model);
        public Task<bool> PutPokemons(Guid pokeId, PutFullPokemonsDTO model);
        public Task<bool> DeleteFullPokemons(Guid pokeID);
    }
}
