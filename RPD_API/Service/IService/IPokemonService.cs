using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IPokemonService : IBaseService
    {
        Task<PokemonDetailDTO?> PostFullPokemons(PostFullPokemonsDTO model);

        Task<PokemonsDTO?> PutFullPokemons(Guid pokeId, PutFullPokemonsDTO model);

        Task<bool> DeleteFullPokemons(Guid pokeID);
    }
}
