using RPD_API.DTO.GrowthRate;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IGrowthRateRepo
    {
        public Task<List<GrowthRateDTO>> GetAllGrowthRate();
        public Task<GrowthRateDTO> GetGrowthRateById(Guid growthRateID);
        public Task<GrowthRateDTO?> AddGrowthRate(PostGrowthRateDTO model);
        public Task<bool> UpdateGrowthRate(Guid growthRateID, GrowthRateDTO model);
        public Task<bool> DeleteGrowthRate(Guid growthRateID);
    }
}
