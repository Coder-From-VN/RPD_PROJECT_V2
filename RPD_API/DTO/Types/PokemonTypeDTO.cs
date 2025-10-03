using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PokemonTypeDTO
    {
        public Guid typesID { get; set; }
        public TypesDTO Types { get; set; }
    }
}
