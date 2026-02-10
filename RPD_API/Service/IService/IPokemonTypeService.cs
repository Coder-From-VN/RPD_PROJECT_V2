using RPD_API.DTO.Types;

namespace RPD_API.Service.IService
{
    public interface IPokemonTypeService : IBaseService
    {
         Task AddPokemonType(Guid typesID, Guid pokeID);
         Task UpdatePokemonType(Guid pokeID, ICollection<PutPokemonTypeDTO> model);
        // Task<bool> DeletePokemonType(Guid typesID, Guid pokeID);
    }
}
