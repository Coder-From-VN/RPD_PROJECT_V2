using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPD_API.Models
{
    public class StatType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid stID { get; set; }
        public string stName { get; set; }

        public ICollection<PokemonStats> PokemonStats { get; set; }
    }
}
