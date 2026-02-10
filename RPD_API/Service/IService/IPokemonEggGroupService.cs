using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Service.IService
{
    public interface IPokemonEggGroupService : IBaseRepository
    {
         Task AddPokemonEggGroup(Guid egID, Guid pokeID);
         Task UpdatePokemonEggGroup(Guid pokeID, ICollection<PutPokemonEggGroupDTO> model);
        // Task<bool> DeletePokemonEggGroup(Guid egID, Guid pokeID);
    }
}
