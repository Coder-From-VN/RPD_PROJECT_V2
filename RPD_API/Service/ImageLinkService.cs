using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
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

        public async Task<bool> AddImageLink(PostImageLinkDTO model, Guid pokeID)
        {
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokeIdCheck == null)
                return false;

            var imageLink = _mapper.Map<ImageLink>(model);
            imageLink.pokeID = pokeID;
            imageLink.Pokemons = pokeIdCheck;

            await _uow.ImageLinks.AddAsync(imageLink);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeleteImageLink(Guid imgID)
        {
            var imageLink = await _uow.ImageLinks.GetByIdAsync(imgID);
            if (imageLink == null)
                return false;

            await _uow.ImageLinks.RemoveAsync(imageLink);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdateImageLink(Guid pokeID, ICollection<PutImageLinkDTO> model)
        {
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokeIdCheck == null)
                return false;

            foreach (var dto in model)
            {
                var image = pokeIdCheck.ImageLink
                    .FirstOrDefault(i => i.imgID == dto.imgID);

                if (image != null && !string.IsNullOrWhiteSpace(dto.imgLink))
                {
                    image.imgLink = dto.imgLink;
                }
            }

            return await _uow.SaveAsync() > 0; ;
        }
    }
}
