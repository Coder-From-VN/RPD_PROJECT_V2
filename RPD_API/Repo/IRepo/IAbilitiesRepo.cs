using RPD_API.DTO.Abilities;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IAbilitiesRepo
    {
        public Task<List<AbilitiesDTO>> GetAllAbilities();
        public Task<AbilitiesDTO> GetAbilitiesById(Guid abID);
        public Task<(bool, AbilitiesDTO)> PostAbilities(PostAbilitiesDTO model);
        public Task<bool> PutAbilities(Guid abID, AbilitiesDTO model);
        public Task<bool> DeleteAbilities(Guid abID);
    }
}
