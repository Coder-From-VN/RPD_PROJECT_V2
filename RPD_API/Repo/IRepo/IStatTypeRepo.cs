using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IStatTypeRepo
    {
        public Task<List<StatTypeDTO>> GetAllStatType();
        public Task<StatTypeDTO> GetStatTypeById(Guid statTypeID);
        public Task<Guid> AddStatType(StatTypeDTO model);
        public Task UpdateStatType(Guid statTypeID, StatTypeDTO model);
        public Task DeleteStatType(Guid statTypeID);
    }
}
