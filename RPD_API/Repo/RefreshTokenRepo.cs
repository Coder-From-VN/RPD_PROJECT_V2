using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class RefreshTokenRepo : BaseRepository<RefreshToken>, IRefreshTokenRepo
    {
        public RefreshTokenRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(RefreshToken token)
        {
            await _context.RefreshTokens.AddAsync(token);
        }

        public async Task<RefreshToken?> GetValidAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(x =>
                    x.Token == token &&
                    !x.IsRevoked &&
                    x.ExpiresAt > DateTime.UtcNow);
        }
    }
}
