using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypesService _typesSer;

        public TypeController(ITypesService typesSer)
        {
            _typesSer = typesSer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTypes()
        {
            return Ok(await _typesSer.GetAllTypes());
        }

        [HttpGet("{typesID}")]
        public async Task<IActionResult> GetTypesById(Guid typesID)
        {
            var types = await _typesSer.GetTypesById(typesID);
            return types == null ? NotFound() : Ok(types);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostTypes(PostTypesDTO model)
        {

            var newTypes = await _typesSer.AddTypes(model);
            if (newTypes == null)
                return Conflict("Ability already exists.");

            return Ok(newTypes);

        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{typesID}")]
        public async Task<IActionResult> PutTypes(Guid typesID, [FromBody] PostTypesDTO model)
        {
            return await _typesSer.UpdateTypes(typesID, model) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{typesID}")]
        public async Task<IActionResult> DeleteTypes([FromRoute] Guid typesID)
        {
            return await _typesSer.DeleteTypes(typesID) ? NoContent() : NotFound();
        }

    }
}
