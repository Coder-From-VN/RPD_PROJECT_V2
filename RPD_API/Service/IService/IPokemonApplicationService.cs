using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IPokemonApplicationService : IBaseService
    {
        Task<PokemonDetailDTO?> PostFullPokemons(PostFullPokemonsDTO model);
        Task<bool> PutFullPokemons(Guid pokeId, PutFullPokemonsDTO model);
        ///Task<int> ImportFullPokemonsAsync(IFormFile pokemonFile,List<IFormFile> addOnFiles);
    }
}
