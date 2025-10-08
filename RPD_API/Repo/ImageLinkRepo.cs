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

        public async Task<bool> UpdateImageLink(Guid pokeID, ICollection<PutImageLinkDTO> model)
        {
            var pokemon = await _context.Pokemons
                .Include(p => p.ImageLink)
                .FirstOrDefaultAsync(p => p.pokeID == pokeID);

            if (pokemon == null)
                return false;

            for (int i = 0; i < pokemon.ImageLink.Count && i < model.Count; i++)
            {
                pokemon.ImageLink.ElementAt(i).imgLink = model.ElementAt(i).imgLink;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
