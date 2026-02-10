using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IImageLinkService : IBaseService
    {
        public Task AddImageLink(PostImageLinkDTO model, Guid pokeID);
        public Task UpdateImageLink(Guid pokeID, ICollection<PutImageLinkDTO> model);
        //public Task<bool> DeleteImageLink(Guid imgID);
    }
}
