using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/pokemon/{pokeID}/Stats")]
    [ApiController]
    public class PokemonStatsController : ControllerBase
    {
        private readonly IPokemonStatsService _pokeStatsSer;

        public PokemonStatsController(IPokemonStatsService pokeStatsSer)
        {
            _pokeStatsSer = pokeStatsSer;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostMoreStats(Guid pokeID, PostPokemonStatsDTO model)
        {
            await _pokeStatsSer.AddPokemonStats(pokeID, model);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutPokemonStats(Guid pokeID, ICollection<PutPokemonStatsDTO> model)
        {
            await _pokeStatsSer.UpdatePokemonStats(pokeID, model);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{stID}")]
        public async Task<IActionResult> DeletePokemonStats(Guid pokeID, Guid stID)
        {
            await _pokeStatsSer.DeletePokemonStats(pokeID, stID);
            return NoContent();
        }
    }
}
