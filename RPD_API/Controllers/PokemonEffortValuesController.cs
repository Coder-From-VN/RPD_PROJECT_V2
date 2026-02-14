using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/pokemon/{pokeID}/EffortValues")]
    [ApiController]
    public class PokemonEffortValuesController : ControllerBase
    {
        private readonly IEffortValuesService _evService;

        public PokemonEffortValuesController(IEffortValuesService evService)
        {
            _evService = evService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostEffortValue(Guid pokeID, [FromBody] PostPokemonsEffortValuesDTO model)
        {
            await _evService.PostEffortValues(pokeID,model);
            return StatusCode(201);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutEffortValues(Guid pokeID,[FromBody] ICollection<PutEffortValuesDTO> model)
        {
            await _evService.UpdateEffortValues(pokeID, model);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{evID}")]
        public async Task<IActionResult> DeleteEffortValue(Guid pokeID, Guid evID)
        {
            await _evService.DeleteEffortValues(pokeID, evID);
            return NoContent();
        }
    }
}
