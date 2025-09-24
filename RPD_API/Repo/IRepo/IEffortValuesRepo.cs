using RPD_API.DTO.EffortValues;

namespace RPD_API.Repo.IRepo
{
    public interface IEffortValuesRepo
    {
        public Task<List<EffortValuesDTO>> GetAllEffortValues();
        public Task<EffortValuesDTO> GetEffortValuesById(Guid evID);
        public Task<Guid> AddEffortValues(EffortValuesDTO model);
        public Task UpdateEffortValues(Guid evID, EffortValuesDTO model);
        public Task DeleteEffortValues(Guid evID);
    }
}
