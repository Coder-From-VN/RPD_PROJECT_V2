using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IImageLinkRepo
    {
        public Task<bool> AddImageLink(PostImageLinkDTO model, Guid pokeID);
        public Task<bool> UpdateImageLink(Guid pokeID, ICollection<PutImageLinkDTO> model);
        public Task<bool> DeleteImageLink(Guid imgID);
    }
}
