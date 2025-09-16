using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class EffortValues
    {
        [Key]
        public Guid evID { get; set; }
        public string evStatName { get; set; }
        public int eValues { get; set; }

        public Guid pokeID { get; set; }
        public Pokemons Pokemons { get; set; }
    }
}
