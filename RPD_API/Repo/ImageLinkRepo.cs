using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
using System;

namespace RPD_API.Repo
{
    public class ImageLinkRepo : IImageLinkRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public ImageLinkRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> AddImageLink(ImageLinkDTO model)
        {
            var newImageLink = _mapper.Map<ImageLink>(model);
            _context.ImageLink!.Add(newImageLink);
            await _context.SaveChangesAsync();

            return newImageLink.imgID;
        }

        public async Task DeleteImageLink(Guid imgID)
        {
            var imageLink = _context.ImageLink!.SingleOrDefault(b => b.imgID == imgID);
            if (imageLink != null)
            {
                _context.ImageLink!.Remove(imageLink);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ImageLinkDTO>> GetAllImageLink()
        {
            var imageLink = await _context.ImageLink.ToListAsync();
            return _mapper.Map<List<ImageLinkDTO>>(imageLink);
        }

        public async Task<ImageLinkDTO> GetImageLinkById(Guid imgID)
        {
            var imageLink = await _context.ImageLink!.FindAsync(imgID);
            return _mapper.Map<ImageLinkDTO>(imageLink);
        }

        public async Task UpdateImageLink(Guid imgID, ImageLinkDTO model)
        {
            if (imgID == model.imgID)
            {
                var updateImageLink = _mapper.Map<ImageLink>(model);
                _context.ImageLink!.Update(updateImageLink);
                await _context.SaveChangesAsync();
            }
        }
    }
}
