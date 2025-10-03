using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PokemonEggGroupDTO
    {
        public Guid egID { get; set; }
        public EggGroupDTO EggGroup { get; set; }
    }
}
