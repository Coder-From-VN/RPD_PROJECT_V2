//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using RPD_API.DTO;
//using RPD_API.Repo.IRepo;

//namespace RPD_API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ImageLinkController : ControllerBase
//    {
//        private readonly IImageLinkRepo _imgRepo;

//        public ImageLinkController(IImageLinkRepo imgRepo)
//        {
//            _imgRepo = imgRepo;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetImageLink()
//        {
//            try
//            {
//                return Ok(await _imgRepo.GetAllImageLink());
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [HttpGet("{imgID}")]
//        public async Task<IActionResult> GetImageLink(Guid imgID)
//        {
//            var imageLink = await _imgRepo.GetImageLinkById(imgID);
//            return imageLink == null ? NotFound() : Ok(imageLink);
//        }

//        [HttpPost]
//        public async Task<IActionResult> PostImageLink(PostImageLinkDTO model)
//        {
//            try
//            {
//                var newImgID = await _imgRepo.AddImageLink(model);
//                return newImgID == null ? NotFound() : Ok(newImgID);
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [HttpPut("{imgID}")]
//        public async Task<IActionResult> UpdateImageLink(Guid imgID, [FromBody] ImageLinkDTO model)
//        {
//            if (imgID != model.imgID)
//            {
//                return NotFound();
//            }
//            var result = await _imgRepo.UpdateImageLink(imgID, model);
//            return Ok(result);
//        }

//        [HttpDelete("{imgID}")]
//        public async Task<IActionResult> DeleteImageLink([FromRoute] Guid imgID)
//        {
//            var result = await _imgRepo.DeleteImageLink(imgID);
//            return Ok(result);
//        }
//    }
//}
