using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IStatTypeService : IBaseService
    {
        public Task<List<StatTypeDTO>> GetAllStatType();
        public Task<StatTypeDTO> GetStatTypeById(Guid statTypeID);
        public Task<StatTypeDTO> AddStatType(PostStatTypeDTO model);
        public Task<bool> UpdateStatType(Guid statTypeID, PutStatTypeDTO model);
        public Task<bool> DeleteStatType(Guid statTypeID);
        Task<int> ImportStatTypeAsync(IFormFile file);
    }
}
