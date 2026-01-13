using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IMoveService : IBaseService
    {
        public Task<List<MoveDTO>> GetAllMove();
        public Task<MoveDTO> GetMoveById(Guid moveID);
        public Task<MoveDTO> AddMove(PostMoveDTO model);
        public Task<bool> UpdateMove(Guid moveID, PutMoveDTO model);
        public Task<bool> DeleteMove(Guid moveID);
    }
}
