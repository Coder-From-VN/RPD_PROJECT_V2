using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using RPD_API.DTO.Admin;
using RPD_API.DTO.Trainer;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RPD_API.Service
{
    public class TrainersService : BaseService, ITrainersService
    {
        private readonly IConfiguration _config;

        public TrainersService(IUnitOfWorkRepo uow, IMapper mapper, IConfiguration config, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
            _config = config;
        }

        public async Task<TrainerLoginResponseDTO> LoginAsync(string firebaseIdToken)
        {
            // 1️⃣ Verify Firebase token
            FirebaseToken decoded =
                await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(firebaseIdToken);

            var uid = decoded.Uid;
            var email = decoded.Claims["email"]?.ToString() ?? "temp eamil";

            // 2️⃣ Find / create trainer
            var trainer = await _uow.Trainers.GetByFirebaseUidAsync(uid);

            if (trainer == null)
            {
                trainer = new Trainer
                {
                    FirebaseUid = uid,
                    tnEmail = email,
                    tnName = decoded.Claims["name"]?.ToString() ?? "tmp name",
                    tnPhotoUrl = decoded.Claims["picture"]?.ToString() ?? "https://upload.wikimedia.org/wikipedia/en/d/d4/Pokemon-Ditto-Artwork.png",
                };

                await _uow.Trainers.AddAsync(trainer);
                await _uow.SaveAsync();
            }

            // 3️⃣ Generate API JWT
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var expireDays = int.Parse(_config["Jwt:ExpireDays"]!);

            var claims = new[]
            {
            new Claim("TrainerId", trainer.TrainerId.ToString()),
            new Claim(ClaimTypes.Email, trainer.tnEmail),
            new Claim(ClaimTypes.Role, "Trainer")
        };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(expireDays),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TrainerLoginResponseDTO
            {
                AccessToken = tokenHandler.WriteToken(token),
                ExpiresAt = tokenDescriptor.Expires!.Value
            };
        }


        public async Task<TrainerLoginResponseDTO> GenerateAdminJwt(AdminLoginDTO dto)
        {
            var username = _config["AdminAuth:Username"];
            var password = _config["AdminAuth:Password"];

            if (dto.Username != username || dto.Password != password)
                return null;

            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
            var expireDays = int.Parse(_config["Jwt:ExpireDays"]!);

            var claims = new[]
            {
        new Claim(ClaimTypes.Name, "Admin"),
        new Claim(ClaimTypes.Role, "Admin") // 🔥 THIS IS THE KEY
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(expireDays),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            return new TrainerLoginResponseDTO
            {
                AccessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                ExpiresAt = tokenDescriptor.Expires!.Value
            };
        }
    }
}