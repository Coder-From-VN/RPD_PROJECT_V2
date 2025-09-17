using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class GrowthRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid growthRateID { get; set; }
        public string grName { get; set; }
        public double grTotalExp { get; set; }

        public ICollection<Pokemons> Pokemons { get; set; }
    }
}
