using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IImageLinkService : IBaseService
    {
        public Task ImageLinkAddOn(Guid pokeID,PostImageLinkDTO model);
        public Task<bool> PostImageLink(Guid pokeID, PostImageLinkDTO model);
        public Task<bool> UpdateImageLink(Guid pokeID, ICollection<PutImageLinkDTO> model);
        public Task<bool> DeleteImageLink(Guid pokeID, Guid imgID);
    }
}
