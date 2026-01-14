using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Service.IService
{
    public interface IPokemonEggGroupService : IBaseRepository
    {
        public Task<bool> AddPokemonEggGroup(Guid egID, Guid pokeID);
        public Task<bool> UpdatePokemonEggGroup(Guid pokeID, ICollection<PutPokemonEggGroupDTO> model);
        public Task<bool> DeletePokemonEggGroup(Guid egID, Guid pokeID);
    }
}
