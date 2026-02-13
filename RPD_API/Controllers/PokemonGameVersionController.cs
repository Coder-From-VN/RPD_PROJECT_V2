using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/pokemon/{pokeID}/GameVersion")]
    [ApiController]
    public class PokemonGameVersionController : ControllerBase
    {
        private readonly IPokemonGameVersionService _pokeGvSer;

        public PokemonGameVersionController(IPokemonGameVersionService pokeGvSer)
        {
            _pokeGvSer = pokeGvSer;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostMoreGameVersion(Guid pokeID, PostPokemonGameVersionDTO model)
        {
            await _pokeGvSer.PostPokemonGameVersion(pokeID, model);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> PutPokemonGameVersion(Guid pokeID, ICollection<PutPokemonGameVersionDTO> model)
        {
            await _pokeGvSer.UpdatePokemonGameVersion(pokeID, model);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{abID}")]
        public async Task<IActionResult> DeletePokemonGameVersion(Guid pokeID, Guid abID)
        {
            await _pokeGvSer.DeletePokemonGameVersion(pokeID, abID);
            return NoContent();
        }
    }
}
