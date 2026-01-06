using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvolutionChartController : ControllerBase
    {
        private readonly IEvolutionChartRepo _ecRepo;

        public EvolutionChartController(IEvolutionChartRepo ecRepo)
        {
            _ecRepo = ecRepo;
        }

        [HttpPost]
        public async Task<IActionResult> PostEvolutionChart(PostEvolutionChartDTO model)
        {
            try
            {
                var newEg = await _ecRepo.PostEvolutionChart(model);

                return newEg == null ? NotFound("Evolution Chart Post fail") : Ok(newEg);
            }
            catch
            {
                return BadRequest(new { message = "Something off at Evolution Chart controller" });
            }
        }

        [HttpDelete("{pokeID,prePokeID}")]
        public async Task<IActionResult> DeleteEggGroup([FromRoute] Guid pokeID, Guid prePokeID)
        {
            var output = await _ecRepo.DeleteEvolutionChart(pokeID, prePokeID);
            return Ok(output);
        }
    }
}
