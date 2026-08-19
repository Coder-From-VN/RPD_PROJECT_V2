using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.DTO.Move;
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
        public async Task<IActionResult> PostPokemonMove(Guid pokeId, List<PostPokemonMoveListItem> model)
        {
            var result = await _pmSer.AddPokemonMove(pokeId, model);
            return Ok($"ADD {result} move to Pokemon");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{pokeID}/{moveID}")]
        public async Task<IActionResult> UpdatePokemonMove(Guid pokeID,Guid moveID,PutPokemonMoveDTO model)
        {
            await _pmSer.UpdatePokemonMove(pokeID, moveID, model);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{pokeID}/{moveID}")]
        public async Task<IActionResult> DeletePokemonMove([FromRoute] Guid pokeID, [FromRoute] Guid moveID)
        {
            return await _pmSer.DeletePokemonMove(pokeID, moveID) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadPokemonMove(Guid pokeID, IFormFile file)
        {
            var count = await _pmSer.ImportPokemonMoveAsync(pokeID,file);
            return Ok(new { imported = count });
        }
    }
}
