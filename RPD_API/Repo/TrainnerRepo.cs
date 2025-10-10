using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RPD_API.DTO.Trainner;
using RPD_API.Helper;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RPD_API.Repo
{
    public class TrainnerRepo : ITrainnerRepo
    {
        private readonly UserManager<Trainner> userManager;
        private readonly SignInManager<Trainner> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;

        public TrainnerRepo(UserManager<Trainner> userManager, SignInManager<Trainner> signInManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;

        }

        public async Task<string> SignInAsync(SignInDTO model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);

            if (user == null || !passwordValid)
            {
                return string.Empty;
            }

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpDTO model)
        {
            var user = new Trainner
            {
                TrainnerName = model.FirstName,
                Email = model.Email,
                UserName = model.Email
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(RPD_ROLE.Trainner))
                {
                    await roleManager.CreateAsync(new IdentityRole(RPD_ROLE.Trainner));
                }

                await userManager.AddToRoleAsync(user, RPD_ROLE.Trainner);
            }
            return result;
        }
    }
}
