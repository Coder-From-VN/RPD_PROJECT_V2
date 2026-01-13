using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IGrowthRateService : IBaseService
    {
        public Task<List<GrowthRateDTO>> GetAllGrowthRate();
        public Task<GrowthRateDTO> GetGrowthRateById(Guid growthRateID);
        public Task<GrowthRateDTO?> AddGrowthRate(PostGrowthRateDTO model);
        public Task<bool> UpdateGrowthRate(Guid growthRateID, PutGrowthRateDTO model);
        public Task<bool> DeleteGrowthRate(Guid growthRateID);
    }
}
