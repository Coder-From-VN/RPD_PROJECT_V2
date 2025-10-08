using RPD_API.DTO;
using RPD_API.DTO.Types;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonTypeRepo
    {
        public Task<bool> AddPokemonType(Guid typesID, Guid pokeID);
        public Task<bool> UpdatePokemonType(Guid pokeID, ICollection<PutPokemonTypeDTO> model);
        public Task<bool> DeletePokemonType(Guid typesID, Guid pokeID);
    }
}
