using RPD_API.DTO.Types;

namespace RPD_API.Service.IService
{
    public interface IPokemonTypeService : IBaseService
    {
        public Task<bool> AddPokemonType(Guid typesID, Guid pokeID);
        public Task<bool> UpdatePokemonType(Guid pokeID, ICollection<PutPokemonTypeDTO> model);
        public Task<bool> DeletePokemonType(Guid typesID, Guid pokeID);
    }
}
