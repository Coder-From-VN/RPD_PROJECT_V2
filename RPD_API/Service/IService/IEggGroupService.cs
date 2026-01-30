using RPD_API.DTO;
using RPD_API.Pagination;

namespace RPD_API.Service.IService
{
    public interface IEggGroupService : IBaseService
    {
        Task<PagedResult<EggGroupDTO>> GetAllEggGroup(QueryParams queryParams);
        Task<EggGroupDTO> GetEggGroupById(Guid egID);
        Task<EggGroupDTO?> AddEggGroup(PostEggGroupDTO model);
        Task<bool> UpdateEggGroup(Guid egID, PutEggGroupDTO model);
        Task<bool> DeleteEggGroup(Guid egID);
    }
}
