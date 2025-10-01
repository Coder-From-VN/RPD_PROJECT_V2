using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PostPokemonDTO
    {
        public int pokeNationalNumber { get; set; }
        public string pokeName { get; set; }
        public string pokeDescription { get; set; }
        public string pokeSpecies { get; set; }
        public decimal pokeHeight { get; set; }
        public decimal pokeWidth { get; set; }
        public double pokeCatchRate { get; set; }
        public int pokeBaseFriendship { get; set; }
        public int pokeBaseExp { get; set; }
        public double pokeMaleRate { get; set; }
        public double pokeFemaleRate { get; set; }
        public int pokeEggCycles { get; set; }
        public int pokeState { get; set; }

        public Guid growthRateID { get; set; }

        public ICollection<PostImageLinkDTO> ImageLink { get; set; }

        public ICollection<PostPokemonsEffortValuesDTO> EffortValues { get; set; }

        public ICollection<PostPokemonStatsDTO> PokemonStats { get; set; }

        public ICollection<PostPokemonAbilitiesDTO> PokemonAbilities { get; set; }

        public ICollection<PostPokemonGameVersionDTO> PokemonGameVersion { get; set; }

        public ICollection<PostPokemonEggGroupDTO> PokemonEggGroup { get; set; }

        public ICollection<PostPokemonTypeDTO> PokemonType { get; set; }

        public ICollection<PostPokemonMoveDTO> PokemonMove { get; set; }
    }
}
