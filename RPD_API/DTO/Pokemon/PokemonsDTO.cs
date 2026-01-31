using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PokemonsDTO
    {
        public Guid pokeID { get; set; }
        public int pokeNationalNumber { get; set; }
        public string pokeName { get; set; }

        //Image 
        public ICollection<ImageLinkDTO> ImageLink { get; set; }
        //PokemonType 
        public ICollection<PokemonTypeDTO> PokemonType { get; set; }

    }
}
