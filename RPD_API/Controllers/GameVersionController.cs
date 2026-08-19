using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Pagination;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameVersionController : ControllerBase
    {
        private readonly IGameVersionService _gvSer;

        public GameVersionController(IGameVersionService gvSer)
        {
            _gvSer = gvSer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGameVersion([FromQuery] QueryParams query)
        {
            return Ok(await _gvSer.GetAllGameVersion(query));
        }

        [HttpGet("{gvID:guid}")]
        public async Task<IActionResult> GetGameVersionById(Guid gvID)
        {
            return Ok(await _gvSer.GetGameVersionById(gvID));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostGameVersion(PostGameVersionDTO model)
        {
            return Ok(await _gvSer.AddGameVersion(model));
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{gvID}")]
        public async Task<IActionResult> PutGameVersion(Guid gvID, [FromBody] PutGameVersionDTO model)
        {
            return await _gvSer.UpdateGameVersion(gvID, model) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{gvID}")]
        public async Task<IActionResult> DeleteGameVersion([FromRoute] Guid gvID)
        {
            return await _gvSer.DeleteGameVersion(gvID) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadGameVersion(IFormFile file)
        {
            var count = await _gvSer.ImportGameVersionAsync(file);
            return Ok(new { imported = count });
        }
    }
}
