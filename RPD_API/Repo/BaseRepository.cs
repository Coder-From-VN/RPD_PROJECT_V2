using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public abstract class BaseRepository<T> : IBaseRepository
    where T : class
    {
        protected readonly rpdDbContext _context;

        protected BaseRepository(rpdDbContext context)
        {
            _context = context;
        }
    }
}
