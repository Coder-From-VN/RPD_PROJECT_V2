using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IPokemonEggGroupService : IBaseService
    {
        Task PokemonEggGroupAddOn(Guid pokeID, Guid egID);
        Task<bool> PostPokemonEggGroup(Guid pokeID, Guid egID);
        Task<bool> UpdatePokemonEggGroup(Guid pokeID, ICollection<PutPokemonEggGroupDTO> model);
        Task<bool> DeletePokemonEggGroup(Guid egID, Guid pokeID);
    }
}
