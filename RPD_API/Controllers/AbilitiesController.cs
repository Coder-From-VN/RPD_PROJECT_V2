using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo;
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
        public async Task<IActionResult> PostType(AbilitiesDTO model)
        {
            try
            {
                var newAbID = await _abRepo.AddAbilities(model);
                var Abilities = await _abRepo.GetAbilitiesById(newAbID);
                return Abilities == null ? NotFound() : Ok(Abilities);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{abID}")]
        public async Task<IActionResult> UpdateType(Guid abID, [FromBody] AbilitiesDTO model)
        {
            if (abID != model.abID)
            {
                return NotFound();
            }
            await _abRepo.UpdateAbilities(abID, model);
            return Ok();
        }

        [HttpDelete("{abID}")]
        public async Task<IActionResult> DeleteType([FromRoute] Guid abID)
        {
            await _abRepo.DeleteAbilities(abID);
            return Ok();
        }
    }
}
