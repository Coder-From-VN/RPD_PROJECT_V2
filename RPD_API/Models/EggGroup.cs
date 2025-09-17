using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPD_API.Models
{
    public class EggGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid egID { get; set; }
        public string egName { get; set; }

        public ICollection<PokemonEggGroup> PokemonEggGroup { get; set; }
    }
}
