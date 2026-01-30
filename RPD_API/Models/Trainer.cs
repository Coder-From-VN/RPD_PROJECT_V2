using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class Trainer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid TrainerId { get; set; }

        public string FirebaseUid { get; set; } = null!;

        public string tnEmail { get; set; } = null!;
        public string tnName { get; set; } = null!;
        public string? tnPhotoUrl { get; set; }

        public DateTime tnCreatedAt { get; set; } = DateTime.UtcNow;
    }
}
