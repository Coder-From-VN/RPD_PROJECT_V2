namespace RPD_API.Models
{
    public class PokemonType
    {
        public Guid typeID { get; set; }
        public Type Type { get; set; }
        public Guid pokeID { get; set; }
        public Pokemons Pokemons { get; set; }
    }
}
