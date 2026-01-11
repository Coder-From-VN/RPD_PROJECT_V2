using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IAbilitiesService
    {
        public Task<List<AbilitiesDTO>> GetAllAbilities();
        public Task<AbilitiesDTO> GetAbilitiesById(Guid abID);
        public Task<AbilitiesDTO?> PostAbilities(PostAbilitiesDTO model);
        public Task<bool> PutAbilities(Guid abID, PutAbilitiesDTO model);
        public Task<bool> DeleteAbilities(Guid abID);
    }
}
