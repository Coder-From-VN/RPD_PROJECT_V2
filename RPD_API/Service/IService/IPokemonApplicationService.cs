using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IPokemonApplicationService : IBaseService
    {
        Task<PokemonDetailDTO?> PostFullPokemons(PostFullPokemonsDTO model);
        Task<bool> PutPokemons(Guid pokeId, PutFullPokemonsDTO model);
        Task<bool> DeleteFullPokemons(Guid pokeID);
        ///Task<int> ImportFullPokemonsAsync(IFormFile pokemonFile,List<IFormFile> addOnFiles);
    }
}
