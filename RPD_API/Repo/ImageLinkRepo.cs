using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

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

        public async Task<bool> AddImageLink(PostImageLinkDTO model, Guid pokeID)
        {
            var pokeIdCheck = await _context.Pokemons!
                .SingleOrDefaultAsync(p => p.pokeID == pokeID);
            if (pokeIdCheck == null)
                return false;
            ImageLink newImageLink = new ImageLink
            {
                imgLink = model.imgLink,
                pokeID = pokeID,
                Pokemons = pokeIdCheck
            };
            _context.ImageLink!.Add(newImageLink);
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
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

        //public async Task<List<ImageLinkDTO>> GetAllImageLink()
        //{
        //    var imageLink = await _context.ImageLink.Include(m => m.Pokemons).ToListAsync();
        //    return _mapper.Map<List<ImageLinkDTO>>(imageLink);
        //}

        //public async Task<ImageLinkDTO> GetImageLinkById(Guid imgID)
        //{
        //    var imageLink = await _context.ImageLink.Include(m => m.Pokemons).SingleOrDefaultAsync(img => img.imgID == imgID);
        //    return _mapper.Map<ImageLinkDTO>(imageLink);
        //}

        //public async Task<bool> UpdateImageLink(Guid imgID, ImageLinkDTO model)
        //{
        //    if (imgID == model.imgID)
        //    {
        //        var updateImageLink = _mapper.Map<ImageLink>(model);
        //        _context.ImageLink!.Update(updateImageLink);
        //        var saved = await _context.SaveChangesAsync();
        //        return saved > 0 ? true : false;
        //    }
        //    return false;
        //}
    }
}
