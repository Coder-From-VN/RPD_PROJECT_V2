using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EggGroupController : ControllerBase
    {
        private readonly IEggGroupService _egSer;

        public EggGroupController(IEggGroupService egSer)
        {
            _egSer = egSer;
        }

        [HttpGet]
        public async Task<IActionResult> GetEggGroup()
        {
            return Ok(await _egSer.GetAllEggGroup());
        }

        [HttpGet("{egID:guid}")]
        public async Task<IActionResult> GetEggGroupById(Guid egID)
        {
            var result = await _egSer.GetEggGroupById(egID);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostEggGroup(PostEggGroupDTO model)
        {

            var result = await _egSer.AddEggGroup(model);
            if (result == null)
                return Conflict("Ability already exists.");
            return Ok(result);

        }

        [HttpPut("{egID}")]
        public async Task<IActionResult> PutEggGroup(Guid egID, [FromBody] PutEggGroupDTO model)
        {
            return await _egSer.UpdateEggGroup(egID, model) ? NoContent() : NotFound();
        }

        [HttpDelete("{egID}")]
        public async Task<IActionResult> DeleteEggGroup([FromRoute] Guid egID)
        {
            return await _egSer.DeleteEggGroup(egID) ? NoContent() : NotFound();
        }
    }
}
