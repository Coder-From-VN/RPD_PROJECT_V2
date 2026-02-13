using RPD_API.DTO;
using RPD_API.Pagination;

namespace RPD_API.Service.IService
{
    public interface IPokemonService : IBaseService
    {
        Task<PagedResult<PokemonsDTO>> GetAllPokemons(QueryParams query);
        Task<PokemonDetailDTO> GetPokemonsById(Guid pokeID);
        Task<PokemonsDTO?> PostPokemons(PostPokemonDTO model);
        Task<bool> PutPokemons(Guid pokeId, PutPokemonDTO model);
        Task<bool> DeletePokemons(Guid pokeID);

        Task<int> ImportPokemonsAsync(IFormFile file);
    }
}
