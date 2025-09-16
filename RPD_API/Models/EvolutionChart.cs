namespace RPD_API.Models
{
    public class EvolutionChart
    {
        public Guid evoID { get; set; }
        public Guid pokeID { get; set; }
        public Guid prePokeID { get; set; }
        public string evoCondition { get; set; }

        public Pokemons Pokemons { get; set; }
        public Pokemons PrePokemons { get; set; }
    }
}
