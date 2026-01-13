using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IImageLinkRepo : IBaseRepository
    {
        public Task AddAsync(ImageLink model);
        public Task<List<ImageLink>> GetAllAsync();
        public Task<ImageLink?> GetByIdAsync(Guid imgID);
        public Task UpdateAsync(ImageLink model);
        public Task RemoveAsync(ImageLink model);
    }
}
