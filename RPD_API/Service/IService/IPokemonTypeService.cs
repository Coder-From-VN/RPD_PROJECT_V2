using RPD_API.DTO;
using RPD_API.DTO.Types;

namespace RPD_API.Service.IService
{
    public interface IPokemonTypeService : IBaseService
    {
        Task PokemonTypeAddOn(Guid pokeID, PostPokemonTypeDTO model);
        Task<bool> PostPokemonType(Guid pokeID, PostPokemonTypeDTO model);
        Task<bool> UpdatePokemonType(Guid pokeID, ICollection<PutPokemonTypeDTO> model);
        Task<bool> DeletePokemonType(Guid pokeID,Guid typesID);
    }
}
