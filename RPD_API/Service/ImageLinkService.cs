using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class ImageLinkService : BaseService, IImageLinkService
    {
        public ImageLinkService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache, cached)
        {
        }

        public async Task ImageLinkAddOn(Guid pokeID, PostImageLinkDTO model)
        {
            var imageLink = _mapper.Map<ImageLink>(model);
            imageLink.pokeID = pokeID;

            await _uow.ImageLinks.AddAsync(imageLink);
        }

        public async Task<bool> DeleteImageLink(Guid pokeID, Guid imgID)
        {
            var image = await _uow.ImageLinks.GetByIdAsync(imgID);

            if (image == null || image.pokeID != pokeID)
                throw new NotFoundException("Image not found for this Pokemon");

            await _uow.ImageLinks.RemoveAsync(image);

            var saved = await _uow.SaveAsync() > 0;

            if (saved)
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");

            return saved;
        }

        public async Task<bool> PostImageLink(Guid pokeID, PostImageLinkDTO model)
        {
            if (!await _uow.Pokemons.ExistsByPokemonByIdAsync(pokeID))
                throw new NotFoundException($"Pokemon with id {pokeID} not found");

            var image = new ImageLink
            {
                imgLink = model.imgLink.Trim(),
                pokeID = pokeID
            };

            await _uow.ImageLinks.AddAsync(image);

            var saved = await _uow.SaveAsync() > 0;

            if (saved)
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");

            return saved;
        }

        public async Task<bool> UpdateImageLink(Guid pokeID, ICollection<PutImageLinkDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetPokemonWithImagesAsync(pokeID);

            if (pokemon == null)
                throw new NotFoundException($"Pokemon with id {pokeID} not found");

            var imageLookup = model.ToDictionary(m => m.imgID);

            foreach (var image in pokemon.ImageLink)
            {
                if (imageLookup.TryGetValue(image.imgID, out var dto))
                {
                    image.imgLink = dto.imgLink.Trim();
                }
            }

            var saved = await _uow.SaveAsync() > 0;

            if (saved)
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");

            return saved;
        }
    }
}
