using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IEggGroupRepo
    {
        public Task<List<EggGroupDTO>> GetAllEggGroup();
        public Task<EggGroupDTO> GetEggGroupById(Guid egID);
        public Task<Guid> AddEggGroup(EggGroupDTO model);
        public Task UpdateEggGroup(Guid egID, EggGroupDTO model);
        public Task DeleteEggGroup(Guid egID);
    }
}
