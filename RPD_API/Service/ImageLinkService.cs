using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class ImageLinkService : BaseService, IImageLinkService
    {
        public ImageLinkService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task AddImageLink(PostImageLinkDTO model, Guid pokeID)
        {
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokeIdCheck == null)
                throw new NotFoundException($"Pokemon with id {pokeID} Not Found");

            var imageLink = _mapper.Map<ImageLink>(model);
            imageLink.pokeID = pokeID;
            imageLink.Pokemons = pokeIdCheck;

            await _uow.ImageLinks.AddAsync(imageLink);
        }

        //public async Task<bool> DeleteImageLink(Guid imgID)
        //{
        //    var imageLink = await _uow.ImageLinks.GetByIdAsync(imgID);
        //    if (imageLink == null)
        //        throw new NotFoundException($"Image with id {imgID} Not Found");

        //    await _uow.ImageLinks.RemoveAsync(imageLink);
        //    return await _uow.SaveAsync() > 0;
        //}

        public async Task UpdateImageLink(Guid pokeID, ICollection<PutImageLinkDTO> model)
        {
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokeIdCheck == null)
                throw new NotFoundException($"Pokemon with id {pokeID} Not Found");

            foreach (var dto in model)
            {
                var image = pokeIdCheck.ImageLink
                    .FirstOrDefault(i => i.imgID == dto.imgID);

                if (image != null && !string.IsNullOrWhiteSpace(dto.imgLink))
                {
                    image.imgLink = dto.imgLink;
                }
            }
        }
    }
}
