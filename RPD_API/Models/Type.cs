using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class Type
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid typeID { get; set; }
        public string typeName { get; set; }

        public ICollection<PokemonType> PokemonType { get; set; }

        public ICollection<Move> Move { get; set; }
    }
}
