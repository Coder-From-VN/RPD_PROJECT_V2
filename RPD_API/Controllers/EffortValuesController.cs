//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using RPD_API.DTO;
//using RPD_API.Repo.IRepo;

//namespace RPD_API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EffortValuesController : ControllerBase
//    {
//        private readonly IEffortValuesRepo _evRepo;

//        public EffortValuesController(IEffortValuesRepo evRepo)
//        {
//            _evRepo = evRepo;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetEffortValues()
//        {
//            try
//            {
//                return Ok(await _evRepo.GetAllEffortValues());
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [HttpGet("{evID}")]
//        public async Task<IActionResult> GetEffortValues(Guid evID)
//        {
//            var type = await _evRepo.GetEffortValuesById(evID);
//            return type == null ? NotFound() : Ok(type);
//        }

//        [HttpPost]
//        public async Task<IActionResult> PostEffortValues(PostEffortValuesDTO model)
//        {
//            try
//            {
//                var newEvID = await _evRepo.AddEffortValues(model);
//                var effortValues = await _evRepo.GetEffortValuesById(newEvID);
//                return effortValues == null ? NotFound() : Ok(effortValues);
//            }
//            catch
//            {
//                return BadRequest();
//            }
//        }

//        [HttpPut("{evID}")]
//        public async Task<IActionResult> UpdateType(Guid evID, [FromBody] EffortValuesDTO model)
//        {
//            if (evID != model.evID)
//            {
//                return NotFound();
//            }
//            await _evRepo.UpdateEffortValues(evID, model);
//            return Ok();
//        }

//        [HttpDelete("{evID}")]
//        public async Task<IActionResult> DeleteType([FromRoute] Guid evID)
//        {
//            await _evRepo.DeleteEffortValues(evID);
//            return Ok();
//        }
//    }
//}
