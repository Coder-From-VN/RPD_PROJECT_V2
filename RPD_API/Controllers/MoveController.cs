using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Pagination;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoveController : ControllerBase
    {
        private readonly IMoveService _moveSer;

        public MoveController(IMoveService moveSer)
        {
            _moveSer = moveSer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMove([FromQuery] QueryParams query)
        {
            return Ok(await _moveSer.GetAllMove(query));
        }

        [HttpGet("{moveID}")]
        public async Task<IActionResult> GetMoveById(Guid moveID)
        {
            return Ok(await _moveSer.GetMoveById(moveID));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostImageLink(PostMoveDTO model)
        {
            return Ok(await _moveSer.AddMove(model));
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{moveID}")]
        public async Task<IActionResult> PutMove(Guid moveID, [FromBody] PutMoveDTO model)
        {
            return await _moveSer.UpdateMove(moveID, model) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{moveID}")]
        public async Task<IActionResult> DeleteMove([FromRoute] Guid moveID)
        {
            return await _moveSer.DeleteMove(moveID) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadMove(IFormFile file)
        {
            var count = await _moveSer.ImportMoveAsync(file);
            return Ok(new { imported = count });
        }
    }
}
