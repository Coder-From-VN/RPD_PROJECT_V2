using RPD_API.DTO;
using RPD_API.Pagination;

namespace RPD_API.Service.IService
{
    public interface IMoveService : IBaseService
    {
        Task<PagedResult<MoveDTO>> GetAllMove(QueryParams query);
        Task<MoveDTO> GetMoveById(Guid moveID);
        Task<MoveDTO> AddMove(PostMoveDTO model);
        Task<bool> UpdateMove(Guid moveID, PutMoveDTO model);
        Task<bool> DeleteMove(Guid moveID);

        Task<int> ImportMoveAsync(IFormFile file);
    }
}
