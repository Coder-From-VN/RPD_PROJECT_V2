using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/pokemon/{pokeID}/abilities")]
    [ApiController]
    public class PokemonAbilitiesController : ControllerBase
    {
        private readonly IPokemonAbilitiesService _pokeAbilitiesSer;

        public PokemonAbilitiesController(IPokemonAbilitiesService pokeAbilitiesSer)
        {
            _pokeAbilitiesSer = pokeAbilitiesSer;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostPokemonAbilities(Guid pokeID, PostPokemonAbilitiesDTO model)
        {
            await _pokeAbilitiesSer.AddPokemonAbilities(pokeID, model);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutPokemonAbilities(Guid pokeID, ICollection<PutPokemonAbilitiesDTO> model)
        {
            await _pokeAbilitiesSer.UpdatePokemonAbilities(pokeID, model);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{abID}")]
        public async Task<IActionResult> DeletePokemonAbilities(Guid pokeID, Guid abID)
        {
            await _pokeAbilitiesSer.DeletePokemonAbilities(pokeID, abID);
            return NoContent();
        }
    }
}
