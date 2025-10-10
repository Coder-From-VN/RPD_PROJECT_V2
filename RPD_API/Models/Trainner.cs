using Microsoft.AspNetCore.Identity;

namespace RPD_API.Models
{
    public class Trainner : IdentityUser
    {
        public string TrainnerName { get; set; }
    }
}
