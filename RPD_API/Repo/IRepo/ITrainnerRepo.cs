using Microsoft.AspNetCore.Identity;
using RPD_API.DTO.Trainner;

namespace RPD_API.Repo.IRepo
{
    public interface ITrainnerRepo
    {
        public Task<IdentityResult> SignUpAsync(SignUpDTO model);
        public Task<string> SignInAsync(SignInDTO model);
    }
}
