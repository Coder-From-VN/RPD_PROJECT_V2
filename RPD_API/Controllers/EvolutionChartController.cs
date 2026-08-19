using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvolutionChartController : ControllerBase
    {
        private readonly IEvolutionChartService _evoSer;

        public EvolutionChartController(IEvolutionChartService evoSer)
        {
            _evoSer = evoSer;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostEvolutionChart(PostEvolutionChartDTO model)
        {
            return await _evoSer.PostEvolutionChart(model)
                ? Ok()
                : BadRequest("Failed to create evolution chart entry.");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{pokeID}/{prePokeID}")]
        public async Task<IActionResult> DeleteEvolutionChart([FromRoute] Guid pokeID, [FromRoute] Guid prePokeID)
        {
            return await _evoSer.DeleteEvolutionChart(pokeID, prePokeID) ? NoContent() : NotFound();

        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{pokeID}/{prePokeID}")]
        public async Task<IActionResult> UpdateEvolutionChart(Guid pokeID, Guid prePokeID, PutEvolutionChartDTO model)
        {
            await _evoSer.UpdateEvolutionChart(pokeID, prePokeID, model);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadEvolutionChart(IFormFile file)
        {
            var count = await _evoSer.ImportEvolutionChartAsync(file);
            return Ok(new { imported = count });
        }
    }
}
