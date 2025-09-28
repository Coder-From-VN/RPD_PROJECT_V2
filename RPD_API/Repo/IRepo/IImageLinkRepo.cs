using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IImageLinkRepo
    {
        public Task<List<ImageLinkDTO>> GetAllImageLink();
        public Task<ImageLinkDTO> GetImageLinkById(Guid imgID);
        public Task<ImageLinkDTO> AddImageLink(PostImageLinkDTO model);
        public Task<bool> UpdateImageLink(Guid imgID, ImageLinkDTO model);
        public Task<bool> DeleteImageLink(Guid imgID);
    }
}
