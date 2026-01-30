using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonMoveController : ControllerBase
    {
        private readonly IPokemonMoveService _pmSer;

        public PokemonMoveController(IPokemonMoveService pmSer)
        {
            _pmSer = pmSer;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostPokemonMove(PostPokemonMoveDTO model)
        {
            var result = await _pmSer.AddPokemonMove(model);
            if (result == null)
                return Conflict("Move already exists.");

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{pokeID}")]
        public async Task<IActionResult> PutPokemonMove(Guid pokeID, [FromBody] ICollection<PutPokemonMoveDTO> model)
        {
            return await _pmSer.UpdatePokemonMove(pokeID, model) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{pokeID}/{moveID}")]
        public async Task<IActionResult> DeletePokemonMove([FromRoute] Guid pokeID, [FromRoute] Guid moveID)
        {
            return await _pmSer.DeletePokemonMove(pokeID, moveID) ? NoContent() : NotFound();
        }
    }
}
