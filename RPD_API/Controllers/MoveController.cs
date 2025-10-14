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
        public async Task<IActionResult> GetAllMove()
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
        public async Task<IActionResult> GetMoveById(Guid moveID)
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
        public async Task<IActionResult> PutMove(Guid moveID, [FromBody] PutMoveDTO model)
        {
            var result = await _mRepo.UpdateMove(moveID, model);
            return Ok(result);
        }

        [HttpDelete("{moveID}")]
        public async Task<IActionResult> DeleteMove([FromRoute] Guid moveID)
        {
            var result = await _mRepo.DeleteMove(moveID);
            return Ok(result);
        }
    }
}
