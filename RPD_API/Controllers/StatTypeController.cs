using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatTypeController : ControllerBase
    {
        private readonly IStatTypeService _stSer;

        public StatTypeController(IStatTypeService stSer)
        {
            _stSer = stSer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStatType()
        {
            return Ok(await _stSer.GetAllStatType());
        }

        [HttpGet("{statTypeID}")]
        public async Task<IActionResult> GetStatTypeById(Guid statTypeID)
        {
            var result = await _stSer.GetStatTypeById(statTypeID);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> PostStatType(PostStatTypeDTO model)
        {
            var result = await _stSer.AddStatType(model);
            if (result == null)
                return Conflict("Ability already exists.");

            return Ok(result);
        }

        [HttpPut("{statTypeID}")]
        public async Task<IActionResult> PutGrowthRate(Guid statTypeID, [FromBody] PostStatTypeDTO model)
        {
            return await _stSer.UpdateStatType(statTypeID, model) ? NoContent() : NotFound();
        }

        [HttpDelete("{statTypeID}")]
        public async Task<IActionResult> DeleteGrowthRate([FromRoute] Guid statTypeID)
        {
            return await _stSer.DeleteStatType(statTypeID) ? NoContent() : NotFound();
        }
    }
}
