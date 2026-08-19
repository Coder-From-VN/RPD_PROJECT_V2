using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IRefreshTokenRepo : IBaseRepository
    {
        Task AddAsync(RefreshToken token);
        Task<RefreshToken?> GetValidAsync(string token);
    }
}
