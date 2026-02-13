using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatTypeController : ControllerBase
    {
        private readonly IStatTypeService _stSer;

        public StatTypeController(IStatTypeService stSer)
        {
            _stSer = stSer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStatType()
        {
            return Ok(await _stSer.GetAllStatType());
        }

        [HttpGet("{statTypeID}")]
        public async Task<IActionResult> GetStatTypeById(Guid statTypeID)
        {
            var result = await _stSer.GetStatTypeById(statTypeID);
            return result == null ? NotFound() : Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostStatType(PostStatTypeDTO model)
        {
            var result = await _stSer.AddStatType(model);
            if (result == null)
                return Conflict("Ability already exists.");

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{statTypeID}")]
        public async Task<IActionResult> PutStatType(Guid statTypeID, [FromBody] PutStatTypeDTO model)
        {
            return await _stSer.UpdateStatType(statTypeID, model) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{statTypeID}")]
        public async Task<IActionResult> DeleteStatType([FromRoute] Guid statTypeID)
        {
            return await _stSer.DeleteStatType(statTypeID) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadStatType(IFormFile file)
        {
            var count = await _stSer.ImportStatTypeAsync(file);
            return Ok(new { imported = count });
        }
    }
}
