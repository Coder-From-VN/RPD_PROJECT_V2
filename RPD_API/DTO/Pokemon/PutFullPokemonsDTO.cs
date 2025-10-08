using RPD_API.DTO.Types;

namespace RPD_API.DTO.Pokemon
{
    public class PutFullPokemonsDTO
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
        //Image 
        public ICollection<PutImageLinkDTO> ImageLink { get; set; }
        //EffortValues
        public ICollection<PutEffortValuesDTO> EffortValues { get; set; }
        //PokemonStats
        public ICollection<PutPokemonStatsDTO> PokemonStats { get; set; }
        //PokemonAbilities
        public ICollection<PutPokemonAbilitiesDTO> PokemonAbilities { get; set; }
        //PokemonGameVersion 
        public ICollection<PutPokemonGameVersionDTO> PokemonGameVersion { get; set; }
        //PokemonEggGroup 
        public ICollection<PutPokemonEggGroupDTO> PokemonEggGroup { get; set; }
        //PokemonType 
        public ICollection<PutPokemonTypeDTO> PokemonType { get; set; }
        //PokemonMove
        public ICollection<PutPokemonMoveDTO> PokemonMove { get; set; }
    }
}
