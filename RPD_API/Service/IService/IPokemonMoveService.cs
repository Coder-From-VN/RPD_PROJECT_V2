using RPD_API.DTO;
using RPD_API.DTO.Move;

namespace RPD_API.Service.IService
{
    public interface IPokemonMoveService : IBaseService
    {
        Task<int> AddPokemonMove(Guid pokeID, List<PostPokemonMoveListItem> model);
        Task<bool> UpdatePokemonMove(Guid pokeID, Guid moveID,PutPokemonMoveDTO model);
        Task<bool> DeletePokemonMove(Guid pokeID, Guid moveID);
        Task<int> ImportPokemonMoveAsync(Guid pokeID, IFormFile file);
    }
}
