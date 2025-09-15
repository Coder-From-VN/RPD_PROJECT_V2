namespace RPD_API.Models
{
    public class EvolutionChart
    {
        public Guid evoID { get; set; }
        public Guid evoFromPokeID { get; set; }
        public Guid evoToPokeID { get; set; }
        public string evoCondition { get; set; }

        public Pokemons evoFromPoke { get; set; }
        public Pokemons evoToPoke { get; set; }
    }
}
