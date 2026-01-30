using RPD_API.DTO;
using RPD_API.Pagination;

namespace RPD_API.Service.IService
{
    public interface IGrowthRateService : IBaseService
    {
        Task<PagedResult<GrowthRateDTO>> GetAllGrowthRate(QueryParams query);
        Task<GrowthRateDTO> GetGrowthRateById(Guid growthRateID);
        Task<GrowthRateDTO?> AddGrowthRate(PostGrowthRateDTO model);
        Task<bool> UpdateGrowthRate(Guid growthRateID, PutGrowthRateDTO model);
        Task<bool> DeleteGrowthRate(Guid growthRateID);
    }
}
