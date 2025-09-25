using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO.Abilities;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbilitiesController : ControllerBase
    {
        private readonly IAbilitiesRepo _abRepo;

        public AbilitiesController(IAbilitiesRepo abRepo)
        {
            _abRepo = abRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAbilities()
        {
            try
            {
                return Ok(await _abRepo.GetAllAbilities());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{abID}")]
        public async Task<IActionResult> GetAbilities(Guid abID)
        {
            var type = await _abRepo.GetAbilitiesById(abID);
            return type == null ? NotFound() : Ok(type);
        }

        [HttpPost]
        public async Task<IActionResult> PostAbilities(PostAbilitiesDTO model)
        {
            try
            {
                var newAbID = await _abRepo.PostAbilities(model);

                return newAbID == null ? NotFound("Ability already exists.") : Ok(newAbID);
            }
            catch
            {
                return BadRequest(new { message = "Something off at Ability controller" });
            }
        }

        [HttpPut("{abID}")]
        public async Task<IActionResult> PutAbilities(Guid abID, [FromBody] AbilitiesDTO model)
        {
            if (abID != model.abID)
            {
                return NotFound();
            }
            await _abRepo.PutAbilities(abID, model);
            return Ok();
        }

        [HttpDelete("{abID}")]
        public async Task<IActionResult> DeleteAbilities([FromRoute] Guid abID)
        {
            await _abRepo.DeleteAbilities(abID);
            return Ok();
        }
    }
}
