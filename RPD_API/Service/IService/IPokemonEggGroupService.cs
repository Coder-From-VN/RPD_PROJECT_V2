using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Service.IService
{
    public interface IPokemonEggGroupService : IBaseRepository
    {
        Task PokemonEggGroupAddOn(Guid pokeID, Guid egID);
        Task<bool> PostPokemonEggGroup(Guid pokeID, Guid egID);
        Task<bool> UpdatePokemonEggGroup(Guid pokeID, ICollection<PutPokemonEggGroupDTO> model);
        Task<bool> DeletePokemonEggGroup(Guid egID, Guid pokeID);
    }
}
