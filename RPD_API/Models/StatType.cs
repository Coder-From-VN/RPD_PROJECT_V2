using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class StatType
    {
        [Key]
        public Guid stID { get; set; }
        public string stName { get; set; }

        public ICollection<PokemonStats> PokemonStats { get; set; }
    }
}
