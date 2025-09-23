using RPD_API.DTO.Move;

namespace RPD_API.Repo.IRepo
{
    public interface IMoveRepo
    {
        public Task<List<MoveDTO>> GetAllMove();
        public Task<MoveDTO> GetMoveById(Guid moveID);
        public Task<Guid> AddMove(PostMoveDTO model);
        public Task UpdateMove(Guid moveID, MoveDTO model);
        public Task DeleteMove(Guid moveID);
    }
}
