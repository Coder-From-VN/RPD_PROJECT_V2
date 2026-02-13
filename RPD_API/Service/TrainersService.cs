using AutoMapper;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using RPD_API.Caching;
using RPD_API.DTO.Admin;
using RPD_API.DTO.Trainer;
using RPD_API.Middleware.Exceptions;
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

        public TrainersService(
            IUnitOfWorkRepo uow, 
            IMapper mapper, 
            IConfiguration config, 
            IDistributedCache cache,
            ICacheService cached
            )
        : base(uow, mapper, cache, cached)
        {
            _config = config;
        }

        public async Task<TrainerLoginResponseDTO> LoginAsync(string firebaseIdToken)
        {
            var decoded = await FirebaseAuth.DefaultInstance
        .VerifyIdTokenAsync(firebaseIdToken);

            var trainer = await _uow.Trainers.GetByFirebaseUidAsync(decoded.Uid);

            if (trainer == null)
            {
                trainer = new Trainer
                {
                    FirebaseUid = decoded.Uid,
                    tnEmail = decoded.Claims["email"]?.ToString(),
                    tnName = decoded.Claims["name"]?.ToString()
                };

                await _uow.Trainers.AddAsync(trainer);
                await _uow.SaveAsync();
            }

            var accessExpire = DateTime.UtcNow.AddMinutes(10);
            var refreshExpire = DateTime.UtcNow.AddDays(7);

            var accessToken = GenerateAccessToken(trainer, accessExpire);
            var refreshToken = GenerateRefreshToken();

            await _uow.RefreshTokens.AddAsync(new RefreshToken
            {
                TrainerId = trainer.TrainerId,
                Token = refreshToken,
                ExpiresAt = refreshExpire
            });

            await _uow.SaveAsync();

            return new TrainerLoginResponseDTO
            {
                AccessToken = accessToken,
                ExpiresAt = accessExpire,
                RefreshToken = refreshToken
            };
        }

        public async Task<TrainerLoginResponseDTO> GenerateAdminJwt(AdminLoginDTO dto)
        {
            var username = _config["AdminAuth:Username"];
            var password = _config["AdminAuth:Password"];

            if (dto.Username != username || dto.Password != password)
                throw new BadRequestException("User name and password can not be empty");

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

        public async Task<TrainerLoginResponseDTO> RefreshAsync(string refreshToken)
        {
            var stored = await _uow.RefreshTokens.GetValidAsync(refreshToken);

            if (stored == null)
                throw new ForbiddenException("Invalid refresh token");

            stored.IsRevoked = true;

            var trainer = await _uow.Trainers.GetByIdAsync(stored.TrainerId);

            var newAccessExpire = DateTime.UtcNow.AddMinutes(10);
            var newRefreshToken = GenerateRefreshToken();

            await _uow.RefreshTokens.AddAsync(new RefreshToken
            {
                TrainerId = trainer!.TrainerId,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await _uow.SaveAsync();

            return new TrainerLoginResponseDTO
            {
                AccessToken = GenerateAccessToken(trainer, newAccessExpire),
                ExpiresAt = newAccessExpire,
                RefreshToken = newRefreshToken
            };
        }


        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(
                System.Security.Cryptography.RandomNumberGenerator.GetBytes(64)
            );
        }

        private string GenerateAccessToken(Trainer trainer, DateTime expiresAt)
        {
            var claims = new[]
            {
        new Claim("TrainerId", trainer.TrainerId.ToString()),
        new Claim(ClaimTypes.Email, trainer.tnEmail),
        new Claim(ClaimTypes.Role, "Trainer")
    };

            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)
            };

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(handler.CreateToken(tokenDescriptor));
        }

    }
}