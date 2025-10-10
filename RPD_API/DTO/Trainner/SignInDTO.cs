using System.ComponentModel.DataAnnotations;

namespace RPD_API.DTO.Trainner
{
    public class SignInDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
