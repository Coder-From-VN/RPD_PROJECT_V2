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
            return Ok(await _typesSer.GetTypesById(typesID));
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> PostTypes(PostTypesDTO model)
        {
            return Ok(await _typesSer.AddTypes(model));
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{typesID}")]
        public async Task<IActionResult> PutTypes(Guid typesID, [FromBody] PutTypesDTO model)
        {
            return await _typesSer.UpdateTypes(typesID, model) ? NoContent() : NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{typesID}")]
        public async Task<IActionResult> DeleteTypes([FromRoute] Guid typesID)
        {
            return await _typesSer.DeleteTypes(typesID) ? NoContent() : NotFound();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadTypes(IFormFile file)
        {
            var count = await _typesSer.ImportTypesAsync(file);
            return Ok(new { imported = count });
        }
    }
}
