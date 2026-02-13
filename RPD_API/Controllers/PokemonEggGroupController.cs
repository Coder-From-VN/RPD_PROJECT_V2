using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/pokemon/{pokeID}/EggGroup")]
    [ApiController]
    public class PokemonEggGroupController : ControllerBase
    {
        private readonly IPokemonEggGroupService _pokeegSer;

        public PokemonEggGroupController(IPokemonEggGroupService pokeegSer)
        {
            _pokeegSer = pokeegSer;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostMoreEggGroup(Guid pokeID,Guid egID)
        {
            await _pokeegSer.PostPokemonEggGroup(pokeID, egID);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutPokemonEggGroup(Guid pokeID, ICollection<PutPokemonEggGroupDTO> model)
        {
            await _pokeegSer.UpdatePokemonEggGroup(pokeID, model);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{egID}")]
        public async Task<IActionResult> DeletePokemonEggGroup(Guid egID, Guid pokeID)
        {
            await _pokeegSer.DeletePokemonEggGroup(egID, pokeID);
            return NoContent();
        }
    }
}
