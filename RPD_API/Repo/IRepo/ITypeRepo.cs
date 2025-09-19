using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface ITypeRepo
    {
        public Task<List<TypeDTO>> GetAllType();
        public Task<TypeDTO> GetTypeById(Guid typeID);
        public Task<Guid> AddType(TypeDTO model);
        public Task UpdateType(Guid typeID, TypeDTO model);
        public Task DeleteType(Guid typeID);
    }
}
