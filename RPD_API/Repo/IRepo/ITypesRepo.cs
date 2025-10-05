using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface ITypesRepo
    {
        public Task<List<TypesDTO>> GetAllTypes();
        public Task<TypesDTO> GetTypesById(Guid typesID);
        public Task<TypesDTO> AddTypes(PostTypesDTO model);
        public Task<bool> UpdateTypes(Guid typesID, PostTypesDTO model);
        public Task<bool> DeleteTypes(Guid typesID);
    }
}
