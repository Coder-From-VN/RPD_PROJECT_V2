using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IImageLinkRepo
    {
        public Task<List<ImageLinkDTO>> GetAllImageLink();
        public Task<ImageLinkDTO> GetImageLinkById(Guid imgID);
        public Task<Guid> AddImageLink(ImageLinkDTO model);
        public Task UpdateImageLink(Guid imgID, ImageLinkDTO model);
        public Task DeleteImageLink(Guid imgID);
    }
}
