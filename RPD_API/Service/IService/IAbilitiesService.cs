using RPD_API.DTO;
using RPD_API.Pagination;

namespace RPD_API.Service.IService
{
    public interface IAbilitiesService : IBaseService
    {
        Task<AbilitiesDTO?> PostAbilities(PostAbilitiesDTO model);
        Task<PagedResult<AbilitiesDTO>> GetAllAbilities(QueryParams query);
        Task<AbilitiesDTO> GetAbilitiesById(Guid abID);
        Task<bool> PutAbilities(Guid abID, PutAbilitiesDTO model);
        Task<bool> DeleteAbilities(Guid abID);
        Task<int> ImportAbilitiesAsync(IFormFile file);
    }
}
