using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RPD_API.DTO;
using RPD_API.DTO.Trainner;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace RPD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainnerController : ControllerBase
    {
        private readonly ITrainnerRepo _tnRepo;

        public TrainnerController(ITrainnerRepo repo)
        {
            _tnRepo = repo;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(SignUpDTO signUpModel)
        {
            var result = await _tnRepo.SignUpAsync(signUpModel);
            if (result.Succeeded)
            {
                return Ok(result.Succeeded);
            }

            return StatusCode(500);
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(SignInDTO signInModel)
        {
            var result = await _tnRepo.SignInAsync(signInModel);

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized();
            }

            return Ok(result);
        }
    }
}
