using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatTypeController : ControllerBase
    {
        private readonly IStatTypeRepo _stRepo;

        public StatTypeController(IStatTypeRepo stRepo)
        {
            _stRepo = stRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStatType()
        {
            try
            {
                return Ok(await _stRepo.GetAllStatType());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{statTypeID}")]
        public async Task<IActionResult> GetStatTypeById(Guid statTypeID)
        {
            var growthRate = await _stRepo.GetStatTypeById(statTypeID);
            return growthRate == null ? NotFound() : Ok(growthRate);
        }

        [HttpPost]
        public async Task<IActionResult> PostStatType(PostStatTypeDTO model)
        {
            try
            {
                var newStatTypeID = await _stRepo.AddStatType(model);
                return newStatTypeID == null ? NotFound("StatType Exit") : Ok(newStatTypeID);
            }
            catch
            {
                return BadRequest("Some thing worng at StatType controller");
            }
        }

        [HttpPut("{statTypeID}")]
        public async Task<IActionResult> PutGrowthRate(Guid statTypeID, [FromBody] PostStatTypeDTO model)
        {
            var result = await _stRepo.UpdateStatType(statTypeID, model);
            return Ok(result);
        }

        [HttpDelete("{statTypeID}")]
        public async Task<IActionResult> DeleteGrowthRate([FromRoute] Guid statTypeID)
        {
            var result = await _stRepo.DeleteStatType(statTypeID);
            return Ok(result);
        }
    }
}
