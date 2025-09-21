using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EggGroupController : ControllerBase
    {
        private readonly IEggGroupRepo _egRepo;

        public EggGroupController(IEggGroupRepo egRepo)
        {
            _egRepo = egRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetEggGroup()
        {
            try
            {
                return Ok(await _egRepo.GetAllEggGroup());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{egID}")]
        public async Task<IActionResult> GetEggGroup(Guid egID)
        {
            var eg = await _egRepo.GetEggGroupById(egID);
            return eg == null ? NotFound() : Ok(eg);
        }

        [HttpPost]
        public async Task<IActionResult> PostType(EggGroupDTO model)
        {
            try
            {
                var newEgID = await _egRepo.AddEggGroup(model);
                var Abilities = await _egRepo.GetEggGroupById(newEgID);
                return Abilities == null ? NotFound() : Ok(Abilities);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{egID}")]
        public async Task<IActionResult> UpdateType(Guid egID, [FromBody] EggGroupDTO model)
        {
            if (egID != model.egID)
            {
                return NotFound();
            }
            await _egRepo.UpdateEggGroup(egID, model);
            return Ok();
        }

        [HttpDelete("{egID}")]
        public async Task<IActionResult> DeleteType([FromRoute] Guid egID)
        {
            await _egRepo.DeleteEggGroup(egID);
            return Ok();
        }
    }
}
