using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO.ImageLink;
using RPD_API.DTO.Move;
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

        public async Task<ImageLinkDTO> AddImageLink(PostImageLinkDTO model)
        {
            var newImageLink = _mapper.Map<ImageLink>(model);
            _context.ImageLink!.Add(newImageLink);
            var saved = await _context.SaveChangesAsync();

            if (saved > 0)
            {
                var ImageLinkWithPokemonName = await _context.ImageLink
                    .Include(m => m.Pokemons)
                    .FirstOrDefaultAsync(m => m.imgID == newImageLink.imgID);
                return _mapper.Map<ImageLinkDTO>(ImageLinkWithPokemonName);
            }
            return null;
        }

        public async Task<bool> DeleteImageLink(Guid imgID)
        {
            var imageLink = _context.ImageLink!.SingleOrDefault(b => b.imgID == imgID);
            if (imageLink != null)
            {
                _context.ImageLink!.Remove(imageLink);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }

        public async Task<List<ImageLinkDTO>> GetAllImageLink()
        {
            var imageLink = await _context.ImageLink.Include(m => m.Pokemons).ToListAsync();
            return _mapper.Map<List<ImageLinkDTO>>(imageLink);
        }

        public async Task<ImageLinkDTO> GetImageLinkById(Guid imgID)
        {
            var imageLink = await _context.ImageLink.Include(m => m.Pokemons).SingleOrDefaultAsync(img => img.imgID == imgID);
            return _mapper.Map<ImageLinkDTO>(imageLink);
        }

        public async Task<bool> UpdateImageLink(Guid imgID, ImageLinkDTO model)
        {
            if (imgID == model.imgID)
            {
                var updateImageLink = _mapper.Map<ImageLink>(model);
                _context.ImageLink!.Update(updateImageLink);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }
    }
}
