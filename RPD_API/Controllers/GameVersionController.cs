using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameVersionController : ControllerBase
    {
        private readonly IGameVersionRepo _gvRepo;

        public GameVersionController(IGameVersionRepo gvRepo)
        {
            _gvRepo = gvRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetGameVersion()
        {
            try
            {
                return Ok(await _gvRepo.GetAllGameVersion());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{gvID}")]
        public async Task<IActionResult> GetGameVersion(Guid gvID)
        {
            var gameVersion = await _gvRepo.GetGameVersionById(gvID);
            return gameVersion == null ? NotFound() : Ok(gameVersion);
        }

        [HttpPost]
        public async Task<IActionResult> PostGameVersion(GameVersionDTO model)
        {
            try
            {
                var newGvID = await _gvRepo.AddGameVersion(model);
                var gameVersion = await _gvRepo.GetGameVersionById(newGvID);
                return gameVersion == null ? NotFound() : Ok(gameVersion);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{gvID}")]
        public async Task<IActionResult> UpdateType(Guid gvID, [FromBody] GameVersionDTO model)
        {
            if (gvID != model.gvID)
            {
                return NotFound();
            }
            await _gvRepo.UpdateGameVersion(gvID, model);
            return Ok();
        }

        [HttpDelete("{gvID}")]
        public async Task<IActionResult> DeleteType([FromRoute] Guid gvID)
        {
            await _gvRepo.DeleteGameVersion(gvID);
            return Ok();
        }
    }
}
