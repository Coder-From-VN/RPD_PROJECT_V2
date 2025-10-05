using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IStatTypeRepo
    {
        public Task<List<StatTypeDTO>> GetAllStatType();
        public Task<StatTypeDTO> GetStatTypeById(Guid statTypeID);
        public Task<StatTypeDTO> AddStatType(PostStatTypeDTO model);
        public Task<bool> UpdateStatType(Guid statTypeID, PostStatTypeDTO model);
        public Task<bool> DeleteStatType(Guid statTypeID);
    }
}
