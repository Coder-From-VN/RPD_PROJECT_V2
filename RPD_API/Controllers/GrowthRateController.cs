using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrowthRateController : ControllerBase
    {
        private readonly IGrowthRateRepo _grRepo;

        public GrowthRateController(IGrowthRateRepo grRepo)
        {
            _grRepo = grRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetGrowthRate()
        {
            try
            {
                return Ok(await _grRepo.GetAllGrowthRate());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{growthRateID}")]
        public async Task<IActionResult> GetGrowthRate(Guid growthRateID)
        {
            var growthRate = await _grRepo.GetGrowthRateById(growthRateID);
            return growthRate == null ? NotFound() : Ok(growthRate);
        }

        [HttpPost]
        public async Task<IActionResult> PostGrowthRate(GrowthRateDTO model)
        {
            try
            {
                var newGrowthRateID = await _grRepo.AddGrowthRate(model);
                var growthRate = await _grRepo.GetGrowthRateById(newGrowthRateID);
                return growthRate == null ? NotFound() : Ok(growthRate);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{growthRateID}")]
        public async Task<IActionResult> UpdateGrowthRate(Guid growthRateID, [FromBody] GrowthRateDTO model)
        {
            if (growthRateID != model.growthRateID)
            {
                return NotFound();
            }
            await _grRepo.UpdateGrowthRate(growthRateID, model);
            return Ok();
        }

        [HttpDelete("{growthRateID}")]
        public async Task<IActionResult> DeleteGrowthRate([FromRoute] Guid growthRateID)
        {
            await _grRepo.DeleteGrowthRate(growthRateID);
            return Ok();
        }
    }
}
