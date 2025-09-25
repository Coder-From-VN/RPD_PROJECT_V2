using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO.Types;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypesRepo _typesRepo;

        public TypeController(ITypesRepo typesRepo)
        {
            _typesRepo = typesRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetTypes()
        {
            try
            {
                return Ok(await _typesRepo.GetAllTypes());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{typesID}")]
        public async Task<IActionResult> GetTypes(Guid typesID)
        {
            var types = await _typesRepo.GetTypesById(typesID);
            return types == null ? NotFound() : Ok(types);
        }

        [HttpPost]
        public async Task<IActionResult> PostTypes(PostTypesDTO model)
        {
            try
            {
                var newTypes = await _typesRepo.AddTypes(model);
                return newTypes == null ? NotFound("Types existing") : Ok(newTypes);
            }
            catch
            {
                return BadRequest("Some thing wrong at Types Controller");
            }
        }

        [HttpPut("{typesID}")]
        public async Task<IActionResult> UpdateTypes(Guid typesID, [FromBody] TypesDTO model)
        {
            if (typesID != model.typesID)
            {
                return NotFound();
            }
            var result = await _typesRepo.UpdateTypes(typesID, model);
            return Ok(result);
        }

        [HttpDelete("{typesID}")]
        public async Task<IActionResult> DeleteTypes([FromRoute] Guid typesID)
        {
            var result = await _typesRepo.DeleteTypes(typesID);
            return Ok(result);
        }

    }
}
