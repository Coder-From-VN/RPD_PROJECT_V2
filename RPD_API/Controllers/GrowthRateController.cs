using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO.GrowthRate;
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
        public async Task<IActionResult> PostGrowthRate(PostGrowthRateDTO model)
        {
            try
            {
                var newGrowthRateID = await _grRepo.AddGrowthRate(model);
                return newGrowthRateID == null ? NotFound("Growth Rate already exists.") : Ok(newGrowthRateID);
            }
            catch
            {
                return BadRequest(new { message = "Something off at Growth Rate controller" });
            }
        }

        [HttpPut("{growthRateID}")]
        public async Task<IActionResult> UpdateGrowthRate(Guid growthRateID, [FromBody] GrowthRateDTO model)
        {
            if (growthRateID != model.growthRateID)
            {
                return NotFound();
            }
            var output = await _grRepo.UpdateGrowthRate(growthRateID, model);
            return Ok(output);
        }

        [HttpDelete("{growthRateID}")]
        public async Task<IActionResult> DeleteGrowthRate([FromRoute] Guid growthRateID)
        {
            var output = await _grRepo.DeleteGrowthRate(growthRateID);
            return Ok(output);
        }
    }
}
