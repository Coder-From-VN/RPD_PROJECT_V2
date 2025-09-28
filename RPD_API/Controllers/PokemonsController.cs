using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonsController : ControllerBase
    {
        private readonly IPokemonsRepo _pokeRepo;

        public PokemonsController(IPokemonsRepo pokeRepo)
        {
            _pokeRepo = pokeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetMove()
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
        public async Task<IActionResult> GetAllPokemon(Guid pokeID)
        {
            var poke = await _pokeRepo.GetPokemonsById(pokeID);
            return poke == null ? NotFound() : Ok(poke);
        }

        [HttpPost]
        public async Task<IActionResult> PostPokemons(PostPokemonDTO model)
        {
            try
            {
                var newpoke = await _pokeRepo.AddPokemons(model);
                return newpoke == null ? NotFound("Pokemon existed") : Ok(newpoke);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{pokeID}")]
        public async Task<IActionResult> PutPokemons(Guid pokeID, [FromBody] PokemonsDTO model)
        {
            if (pokeID != model.pokeID)
            {
                return NotFound();
            }
            var result = await _pokeRepo.UpdatePokemons(pokeID, model);
            return Ok(result);
        }

        [HttpDelete("{pokeID}")]
        public async Task<IActionResult> DeletePokemons([FromRoute] Guid pokeID)
        {
            var result = await _pokeRepo.DeletePokemons(pokeID);
            return Ok(result);
        }

    }
}
