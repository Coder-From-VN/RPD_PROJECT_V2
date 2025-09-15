namespace RPD_API.Models
{
    public class PokemonStats
    {
        public Guid stID { get; set; }
        public Guid pokeID { get; set; }
        public int Basevalue { get; set; }
        public int minValue { get; set; }
        public int MaxValue { get; set; }

        public Pokemons Pokemons { get; set; }
        public StatType Type { get; set; }
    }
}
