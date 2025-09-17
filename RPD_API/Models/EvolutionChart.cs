using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class EvolutionChart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid evoID { get; set; }

        public Guid pokeID { get; set; }
        public Guid prePokeID { get; set; }
        public string evoCondition { get; set; }

        public Pokemons Pokemons { get; set; }
        public Pokemons PrePokemons { get; set; }
    }
}
