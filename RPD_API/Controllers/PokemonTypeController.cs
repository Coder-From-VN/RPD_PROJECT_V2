using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.DTO.Types;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/pokemon/{pokeID}/Types")]
    [ApiController]
    public class PokemonTypeController : ControllerBase
    {
        private readonly IPokemonTypeService _pokeTypesSer;

        public PokemonTypeController(IPokemonTypeService pokeTypesSer)
        {
            _pokeTypesSer = pokeTypesSer;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostMoreType(Guid pokeID, PostPokemonTypeDTO model)
        {
            await _pokeTypesSer.PostPokemonType(pokeID, model);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutPokemonTypes(Guid pokeID, ICollection<PutPokemonTypeDTO> model)
        {
            await _pokeTypesSer.UpdatePokemonType(pokeID, model);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{typesID}")]
        public async Task<IActionResult> DeletePokemonTypes(Guid pokeID, Guid typesID)
        {
            await _pokeTypesSer.DeletePokemonType(pokeID, typesID);
            return NoContent();
        }
    }
}
