using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageLinkController : ControllerBase
    {
        private readonly IImageLinkRepo _imgRepo;

        public ImageLinkController(IImageLinkRepo imgRepo)
        {
            _imgRepo = imgRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetImageLink()
        {
            try
            {
                return Ok(await _imgRepo.GetAllImageLink());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{imgID}")]
        public async Task<IActionResult> GetImageLink(Guid imgID)
        {
            var imageLink = await _imgRepo.GetImageLinkById(imgID);
            return imageLink == null ? NotFound() : Ok(imageLink);
        }

        [HttpPost]
        public async Task<IActionResult> PostImageLink(ImageLinkDTO model)
        {
            try
            {
                var newImgID = await _imgRepo.AddImageLink(model);
                var imageLink = await _imgRepo.GetImageLinkById(newImgID);
                return imageLink == null ? NotFound() : Ok(imageLink);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{imgID}")]
        public async Task<IActionResult> UpdateImageLink(Guid imgID, [FromBody] ImageLinkDTO model)
        {
            if (imgID != model.imgID)
            {
                return NotFound();
            }
            await _imgRepo.UpdateImageLink(imgID, model);
            return Ok();
        }

        [HttpDelete("{imgID}")]
        public async Task<IActionResult> DeleteImageLink([FromRoute] Guid imgID)
        {
            await _imgRepo.DeleteImageLink(imgID);
            return Ok();
        }
    }
}
