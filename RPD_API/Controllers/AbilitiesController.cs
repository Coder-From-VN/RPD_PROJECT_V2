using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbilitiesController : ControllerBase
    {
        private readonly IAbilitiesService _abSer;

        public AbilitiesController(IAbilitiesService abSer)
        {
            _abSer = abSer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAbilities()
        {
            return Ok(await _abSer.GetAllAbilities());
        }

        [HttpGet("{abID:guid}")]
        public async Task<IActionResult> GetAbilities(Guid abID)
        {
            var result = await _abSer.GetAbilitiesById(abID);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostAbilities(PostAbilitiesDTO model)
        {

            var result = await _abSer.PostAbilities(model);
            if (result == null)
                return Conflict("Ability already exists.");

            return Ok(result);

        }

        [HttpPut("{abID}")]
        public async Task<IActionResult> PutAbilities(Guid abID, [FromBody] PutAbilitiesDTO model)
        {
            return await _abSer.PutAbilities(abID, model) ? NoContent() : NotFound();
        }

        [HttpDelete("{abID}")]
        public async Task<IActionResult> DeleteAbilities([FromRoute] Guid abID)
        {
            return await _abSer.DeleteAbilities(abID) ? NoContent() : NotFound();
        }
    }
}
