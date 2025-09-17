using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPD_API.Models
{
    public class GameVersion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid gvID { get; set; }
        public string gvName { get; set; }
        public int gvGen { get; set; }

        public ICollection<PokemonGameVersion> PokemonGameVersion { get; set; }
    }
}
