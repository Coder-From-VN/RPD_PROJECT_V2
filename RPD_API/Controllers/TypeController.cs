using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly ITypeRepo _typeRepo;

        public TypeController(ITypeRepo typeRepo)
        {
            _typeRepo = typeRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetType()
        {
            try
            {
                return Ok(await _typeRepo.GetAllType());
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("{typeID}")]
        public async Task<IActionResult> GetType(Guid typeID)
        {
            var type = await _typeRepo.GetTypeById(typeID);
            return type == null ? NotFound() : Ok(type);
        }

        [HttpPost]
        public async Task<IActionResult> PostType(TypeDTO model)
        {
            try
            {
                var newTypeID = await _typeRepo.AddType(model);
                var Type = await _typeRepo.GetTypeById(newTypeID);
                return Type == null ? NotFound() : Ok(Type);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut("{typeID}")]
        public async Task<IActionResult> UpdateType(Guid typeID, [FromBody] TypeDTO model)
        {
            if (typeID != model.typeID)
            {
                return NotFound();
            }
            await _typeRepo.UpdateType(typeID, model);
            return Ok();
        }

        [HttpDelete("{typeID}")]
        public async Task<IActionResult> DeleteType([FromRoute] Guid typeID)
        {
            await _typeRepo.DeleteType(typeID);
            return Ok();
        }

    }
}
