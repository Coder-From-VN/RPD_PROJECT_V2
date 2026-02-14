using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Pagination;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/Pokemons")]
    [ApiController]
    public class PokemonsController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;
        private readonly IPokemonApplicationService _pokeFullService;

        public PokemonsController(IPokemonService pokemonService, IPokemonApplicationService pokeFullService)
        {
            _pokemonService = pokemonService;
            _pokeFullService = pokeFullService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllPokemonss([FromQuery] QueryParams query)
        {
            return Ok(await _pokemonService.GetAllPokemons(query));
        }

        [HttpGet("{pokeID}")]
        public async Task<IActionResult> GetPokemonById(Guid pokeID)
        {
            var result = await _pokemonService.GetPokemonsById(pokeID);
            return result == null ? NotFound() : Ok(result);
        }
        
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostPokemons([FromBody] PostFullPokemonsDTO model)
        {
            var result = await _pokeFullService.PostFullPokemons(model);
            if (result == null)
                return Conflict("Pokemon already exists.");

            return Ok(result);
        }
        //[Authorize(Roles = "Admin")]
        [HttpPut("{pokeID}")]
        public async Task<IActionResult> PutPokemons(Guid pokeID, [FromBody] PutFullPokemonsDTO model)
        {
            return await _pokeFullService.PutFullPokemons(pokeID, model) ? NoContent() : NotFound();
        }
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{pokeID}")]
        public async Task<IActionResult> DeletePokemons([FromRoute] Guid pokeID)
        {
            return await _pokemonService.DeletePokemons(pokeID) ? NoContent() : NotFound();
        }

        //[HttpPost("upload")]
        //[Consumes("multipart/form-data")]
        //public async Task<IActionResult> UploadPokemons(IFormFile pokemonFile,List<IFormFile> addOnFileList)
        //{
        //    var count = await _pokeFullService.ImportFullPokemonsAsync(pokemonFile, addOnFileList);
        //    return Ok(new { imported = count });
        //}
    }
}
