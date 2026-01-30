using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Pagination;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GrowthRateController : ControllerBase
    {
        private readonly IGrowthRateService _grSer;

        public GrowthRateController(IGrowthRateService grSer)
        {
            _grSer = grSer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGrowthRate([FromQuery] QueryParams query)
        {
            return Ok(await _grSer.GetAllGrowthRate(query));
        }

        [HttpGet("{growthRateID}")]
        public async Task<IActionResult> GetGrowthRateById(Guid growthRateID)
        {
            var result = await _grSer.GetGrowthRateById(growthRateID);
            return result == null ? NotFound() : Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostGrowthRate(PostGrowthRateDTO model)
        {
            var result = await _grSer.AddGrowthRate(model);
            if (result == null)
                return Conflict("Ability already exists.");

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{growthRateID}")]
        public async Task<IActionResult> PutGrowthRate(Guid growthRateID, [FromBody] PutGrowthRateDTO model)
        {
            return await _grSer.UpdateGrowthRate(growthRateID, model) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{growthRateID}")]
        public async Task<IActionResult> DeleteGrowthRate([FromRoute] Guid growthRateID)
        {
            return await _grSer.DeleteGrowthRate(growthRateID) ? NoContent() : NotFound();
        }
    }
}
