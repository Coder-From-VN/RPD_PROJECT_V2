using System.ComponentModel.DataAnnotations;

namespace RPD_API.Models
{
    public class PokemonMove
    {
        public Guid pokeID { get; set; }
        public Pokemons Pokemons { get; set; }
        public Guid moveID { get; set; }
        public Move Move { get; set; }

        public string pmLearnMethod { get; set; }
        public int pmLearnLevel { get; set; }

    }
}
