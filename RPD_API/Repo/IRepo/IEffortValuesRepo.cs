using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IEffortValuesRepo
    {
        public Task<List<EffortValuesDTO>> GetAllEffortValues();
        public Task<EffortValuesDTO> GetEffortValuesById(Guid evID);
        public Task<EffortValuesDTO> AddEffortValues(PostEffortValuesDTO model);
        public Task<bool> UpdateEffortValues(Guid evID, EffortValuesDTO model);
        public Task<bool> DeleteEffortValues(Guid evID);
    }
}
