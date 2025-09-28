using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IMoveRepo
    {
        public Task<List<MoveDTO>> GetAllMove();
        public Task<MoveDTO> GetMoveById(Guid moveID);
        public Task<MoveDTO> AddMove(PostMoveDTO model);
        public Task<bool> UpdateMove(Guid moveID, MoveDTO model);
        public Task<bool> DeleteMove(Guid moveID);
    }
}
