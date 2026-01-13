using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class ImageLinkRepo : BaseRepository<ImageLink>, IImageLinkRepo
    {
        public ImageLinkRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(ImageLink model)
        {
            await _context.ImageLink.AddAsync(model);
        }

        public async Task<List<ImageLink>> GetAllAsync()
        {
            return await _context.ImageLink!.AsNoTracking().ToListAsync();
        }

        public async Task<ImageLink?> GetByIdAsync(Guid imgID)
        {
            return await _context.ImageLink!
                .FirstOrDefaultAsync(img => img.imgID == imgID);
        }

        public Task UpdateAsync(ImageLink model)
        {
            _context.ImageLink.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(ImageLink model)
        {
            _context.ImageLink.Remove(model);
            return Task.CompletedTask;
        }


    }
}
