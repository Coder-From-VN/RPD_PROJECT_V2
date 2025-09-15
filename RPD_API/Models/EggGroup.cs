using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class EggGroup
    {
        [Key]
        public Guid egID { get; set; }
        public string egName { get; set; }
    }
}
