using System.ComponentModel.DataAnnotations;

namespace RPD_API.DTO.Trainner
{
    public class SignUpDTO
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string ConfirmPassword { get; set; } = null!;
    }
}
