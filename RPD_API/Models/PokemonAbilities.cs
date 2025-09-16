namespace RPD_API.Models
{
    public class PokemonAbilities
    {
        public Guid pokeID { get; set; }
        public Pokemons Pokemons { get; set; }
        public Guid abID { get; set; }
        public Abilities Abilities { get; set; }

        public bool paHiddenCheck { get; set; }
    }
}
