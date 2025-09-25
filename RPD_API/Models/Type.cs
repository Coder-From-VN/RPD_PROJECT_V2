using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class Types
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid typesID { get; set; }
        public string typesName { get; set; }

        public ICollection<PokemonType> PokemonType { get; set; }

        public ICollection<Move> Move { get; set; }
    }
}
