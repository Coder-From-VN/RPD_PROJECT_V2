using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IAbilitiesRepo
    {
        public Task<List<AbilitiesDTO>> GetAllAbilities();
        public Task<AbilitiesDTO> GetAbilitiesById(Guid abID);
        public Task<Guid> AddAbilities(AbilitiesDTO model);
        public Task UpdateAbilities(Guid abID, AbilitiesDTO model);
        public Task DeleteAbilities(Guid abID);
    }
}
