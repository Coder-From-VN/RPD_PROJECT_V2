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
            var result = await _evoSer.PostEvolutionChart(model);
            if (result == null)
                return Conflict("Move already exists.");

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{pokeID}/{prePokeID}")]
        public async Task<IActionResult> DeleteEggGroup([FromRoute] Guid pokeID, [FromRoute] Guid prePokeID)
        {
            return await _evoSer.DeleteEvolutionChart(pokeID, prePokeID) ? NoContent() : NotFound();

        }
    }
}
