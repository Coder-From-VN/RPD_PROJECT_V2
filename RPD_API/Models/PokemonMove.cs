using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class PokemonMove
    {
        [Key]
        public Guid moveID { get; set; }
        public string moveName { get; set; }
        public string moveCategory { get; set; }
        public int movePower { get; set; }
        public int moveAccuracy { get; set; }
        public int movePP { get; set; }
        public int movePriority { get; set; }
        public string moveDescription { get; set; }
        //type
        public Type Type { get; set; }

    }
}
