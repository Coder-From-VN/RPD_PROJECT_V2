using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/pokemon/{pokeID}/images")]
    [ApiController]
    public class PokemonImagesController : ControllerBase
    {
        private readonly IImageLinkService _imageService;

        public PokemonImagesController(IImageLinkService imageService)
        {
            _imageService = imageService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostImage(Guid pokeID,PostImageLinkDTO model)
        {
            var result = await _imageService.PostImageLink(pokeID, model);

            if (!result)
                return BadRequest();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutImages(Guid pokeID, ICollection<PutImageLinkDTO> model)
        {
            var result = await _imageService.UpdateImageLink(pokeID, model);

            if (!result)
                return BadRequest();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{imgID}")]
        public async Task<IActionResult> DeleteImage(Guid pokeID,Guid imgID)
        {
            var result = await _imageService.DeleteImageLink(pokeID, imgID);

            if (!result)
                return BadRequest();

            return NoContent();
        }

    }
}
