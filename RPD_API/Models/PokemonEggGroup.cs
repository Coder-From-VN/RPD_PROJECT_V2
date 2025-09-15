namespace RPD_API.Models
{
    public class PokemonEggGroup
    {
        public Guid egID { get; set; }
        public Guid pokeID { get; set; }
        public Pokemons Pokemons { get; set; }
        public EggGroup EggGroup { get; set; }
    }
}
