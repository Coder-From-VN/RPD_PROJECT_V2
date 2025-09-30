using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PokemonAbilitiesDTO
    {
        public Guid pokeID { get; set; }
        public Guid abID { get; set; }

        public bool paHiddenCheck { get; set; }
    }
}
