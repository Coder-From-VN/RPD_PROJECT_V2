using Microsoft.AspNetCore.Mvc;
using RPD_API.DTO.Admin;
using RPD_API.DTO.Trainer;
using RPD_API.Service.IService;


namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITrainersService _tnSer;


        public AuthController(
            ITrainersService firebaseAuth)
        {
            _tnSer = firebaseAuth;
        }

        [HttpPost("trainer/login")]
        public async Task<IActionResult> TrainerLogin(TrainerLoginDTO dto)
        {
            var result = await _tnSer.LoginAsync(dto.IdToken);
            return Ok(result);
        }

        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin(AdminLoginDTO dto)
        {
            var token = await _tnSer.GenerateAdminJwt(dto);
            return token == null ? Unauthorized("Invalid admin credentials") : Ok(token);
        }
    }
}
