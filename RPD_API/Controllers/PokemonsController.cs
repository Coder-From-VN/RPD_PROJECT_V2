using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> GetAllPokemonss()
        {
            return Ok(await _pokemonService.GetAllPokemons());
        }

        [HttpGet("{pokeID}")]
        public async Task<IActionResult> GetPokemonById(Guid pokeID)
        {
            var result = await _pokemonService.GetPokemonsById(pokeID);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostPokemons([FromBody] PostFullPokemonsDTO model)
        {
            var result = await _pokeFullService.PostPokemons(model);
            if (result == null)
                return Conflict("Move already exists.");

            return Ok(result);
        }
        //nedd fix
        [HttpPut("{pokeID}")]
        public async Task<IActionResult> PutPokemons(Guid pokeID, [FromBody] PutFullPokemonsDTO model)
        {
            return await _pokeFullService.PutPokemons(pokeID, model) ? NoContent() : NotFound();
        }
        //nedd fix
        [HttpDelete("{pokeID}")]
        public async Task<IActionResult> DeletePokemons([FromRoute] Guid pokeID)
        {
            return await _pokeFullService.DeleteFullPokemons(pokeID) ? NoContent() : NotFound();
        }

    }
}
