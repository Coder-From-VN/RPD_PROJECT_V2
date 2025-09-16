using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class GameVersion
    {
        [Key]
        public Guid gvID { get; set; }
        public string gvName { get; set; }
        public int gvGen { get; set; }

        public ICollection<PokemonGameVersion> PokemonGameVersion { get; set; }
    }
}
