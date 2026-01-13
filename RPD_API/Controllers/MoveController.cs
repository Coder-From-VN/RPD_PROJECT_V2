using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;
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
        public async Task<IActionResult> GetAllMove()
        {
            return Ok(await _moveSer.GetAllMove());
        }

        [HttpGet("{moveID}")]
        public async Task<IActionResult> GetMoveById(Guid moveID)
        {
            var result = await _moveSer.GetMoveById(moveID);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostImageLink(PostMoveDTO model)
        {
            var result = await _moveSer.AddMove(model);
            if (result == null)
                return Conflict("Move already exists.");

            return Ok(result);
        }

        [HttpPut("{moveID}")]
        public async Task<IActionResult> PutMove(Guid moveID, [FromBody] PutMoveDTO model)
        {
            return await _moveSer.UpdateMove(moveID, model) ? NoContent() : NotFound();
        }

        [HttpDelete("{moveID}")]
        public async Task<IActionResult> DeleteMove([FromRoute] Guid moveID)
        {
            return await _moveSer.DeleteMove(moveID) ? NoContent() : NotFound();
        }
    }
}
