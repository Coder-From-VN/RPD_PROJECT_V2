using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class ImageLink
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid imgID { get; set; }
        public string imgLink { get; set; }

        public Guid pokeID { get; set; }
        public Pokemons Pokemons { get; set; }
    }
}
