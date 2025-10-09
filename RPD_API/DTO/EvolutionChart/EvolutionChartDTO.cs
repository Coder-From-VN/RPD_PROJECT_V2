using RPD_API.Models;

namespace RPD_API.DTO
{
    public class EvolutionChartDTO
    {
        public Guid evoID { get; set; }

        public Guid prePokeID { get; set; }
        public string prePokemonName { get; set; }
        public string prePokemonImagelink { get; set; }
        public Guid pokeID { get; set; }
        public string PokemonName { get; set; }
        public string PokemonImagelink { get; set; }

        public string evoCondition { get; set; }
    }
}
