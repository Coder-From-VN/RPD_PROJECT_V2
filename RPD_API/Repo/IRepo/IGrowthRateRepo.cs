using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IGrowthRateRepo
    {
        public Task<List<GrowthRateDTO>> GetAllGrowthRate();
        public Task<GrowthRateDTO> GetGrowthRateById(Guid growthRateID);
        public Task<Guid> AddGrowthRate(GrowthRateDTO model);
        public Task UpdateGrowthRate(Guid growthRateID, GrowthRateDTO model);
        public Task DeleteGrowthRate(Guid growthRateID);
    }
}
