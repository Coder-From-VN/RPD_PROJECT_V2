namespace RPD_API.Models
{
    public class PokemonType
    {
        public Guid typesID { get; set; }
        public Types Types { get; set; }
        public Guid pokeID { get; set; }
        public Pokemons Pokemons { get; set; }
    }
}
