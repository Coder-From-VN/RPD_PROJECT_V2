using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO.GameVersion;
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
        public async Task<IActionResult> PostGameVersion(PostGameVersionDTO model)
        {
            try
            {
                var newGvID = await _gvRepo.AddGameVersion(model);
                return newGvID == null ? NotFound("Game Version existing") : Ok(newGvID);
            }
            catch
            {
                return BadRequest("Some thing wrong at Game version Controller");
            }
        }

        [HttpPut("{gvID}")]
        public async Task<IActionResult> UpdateType(Guid gvID, [FromBody] GameVersionDTO model)
        {
            if (gvID != model.gvID)
            {
                return NotFound();
            }
            var output = await _gvRepo.UpdateGameVersion(gvID, model);
            return Ok(output);
        }

        [HttpDelete("{gvID}")]
        public async Task<IActionResult> DeleteType([FromRoute] Guid gvID)
        {
            var output = await _gvRepo.DeleteGameVersion(gvID);
            return Ok(output);
        }
    }
}
