using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IEggGroupRepo
    {
        public Task<List<EggGroupDTO>> GetAllEggGroup();
        public Task<EggGroupDTO> GetEggGroupById(Guid egID);
        public Task<EggGroupDTO?> AddEggGroup(PostEggGroupDTO model);
        public Task<bool> UpdateEggGroup(Guid egID, PutEggGroupDTO model);
        public Task<bool> DeleteEggGroup(Guid egID);
    }
}
