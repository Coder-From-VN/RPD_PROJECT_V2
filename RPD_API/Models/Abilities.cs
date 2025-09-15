using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class Abilities
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid abID { get; set; }
        public string abName { get; set; }
        public string abDescription { get; set; }
        public string abEffect { get; set; }
        public bool abHiddenCheck { get; set; }
    }
}
