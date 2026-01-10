using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonsController : ControllerBase
    {
        private readonly IPokemonsRepo _pokeRepo;
        private readonly IPokemonService _pokemonService;

        public PokemonsController(IPokemonsRepo pokeRepo, IPokemonService pokemonService)
        {
            _pokeRepo = pokeRepo;
            _pokemonService = pokemonService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllPokemonss()
        {
            try
            {
                return Ok(await _pokeRepo.GetAllPokemons());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{pokeID}")]
        public async Task<IActionResult> GetPokemonById(Guid pokeID)
        {
            var poke = await _pokeRepo.GetPokemonsById(pokeID);
            return poke == null ? NotFound() : Ok(poke);
        }

        [HttpPost]
        public async Task<IActionResult> PostPokemons([FromBody] PostFullPokemonsDTO model)
        {
            try
            {
                var newpoke = await _pokemonService.PostFullPokemons(model);
                return newpoke == null ? NotFound("Pokemon existed") : Ok(newpoke);
            }
            catch
            {
                return BadRequest();
            }
        }
        //nedd fix
        [HttpPut("{pokeID}")]
        public async Task<IActionResult> PutPokemons(Guid pokeID, [FromBody] PutFullPokemonsDTO model)
        {
            var result = await _pokemonService.PutFullPokemons(pokeID, model);
            return Ok(result);
        }
        //nedd fix
        [HttpDelete("{pokeID}")]
        public async Task<IActionResult> DeletePokemons([FromRoute] Guid pokeID)
        {
            var result = await _pokemonService.DeleteFullPokemons(pokeID);
            return Ok(result);
        }

    }
}
