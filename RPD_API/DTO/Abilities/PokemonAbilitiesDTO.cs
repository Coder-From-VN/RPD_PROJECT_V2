using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PokemonAbilitiesDTO
    {
        public Guid abID { get; set; }
        public Abilities Abilities { get; set; }
        public bool paHiddenCheck { get; set; }
    }
}
