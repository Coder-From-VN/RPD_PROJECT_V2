using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoveController : ControllerBase
    {
        private readonly IMoveRepo _mRepo;

        public MoveController(IMoveRepo mRepo)
        {
            _mRepo = mRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetMove()
        {
            try
            {
                return Ok(await _mRepo.GetAllMove());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{moveID}")]
        public async Task<IActionResult> GetMove(Guid moveID)
        {
            var move = await _mRepo.GetMoveById(moveID);
            return move == null ? NotFound() : Ok(move);
        }

        [HttpPost]
        public async Task<IActionResult> PostImageLink(PostMoveDTO model)
        {
            try
            {
                var newMove = await _mRepo.AddMove(model);
                return newMove == null ? NotFound("move existed") : Ok(newMove);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{moveID}")]
        public async Task<IActionResult> UpdateImageLink(Guid moveID, [FromBody] MoveDTO model)
        {
            if (moveID != model.moveID)
            {
                return NotFound();
            }
            var result = await _mRepo.UpdateMove(moveID, model);
            return Ok(result);
        }

        [HttpDelete("{moveID}")]
        public async Task<IActionResult> DeleteImageLink([FromRoute] Guid moveID)
        {
            var result = await _mRepo.DeleteMove(moveID);
            return Ok(result);
        }
    }
}
