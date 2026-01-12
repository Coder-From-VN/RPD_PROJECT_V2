using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
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

        [HttpPost]
        public async Task<IActionResult> PostEvolutionChart(PostEvolutionChartDTO model)
        {
            try
            {
                var result = await _evoSer.PostEvolutionChart(model);

                return result
                    ? Ok()
                    : BadRequest("Evolution Chart post failed");
            }
            catch
            {
                return BadRequest(new { message = "Something off at Evolution Chart controller" });
            }
        }

        [HttpDelete("{pokeID}/{prePokeID}")]
        public async Task<IActionResult> DeleteEggGroup([FromRoute] Guid pokeID, [FromRoute] Guid prePokeID)
        {
            var output = await _evoSer.DeleteEvolutionChart(pokeID, prePokeID);
            return output ? Ok() : NotFound();
        }
    }
}
