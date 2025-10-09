using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PokemonsDTO
    {
        public Guid pokeID { get; set; }
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
        public string grName { get; set; }
        //Image 
        public ICollection<ImageLinkDTO> ImageLink { get; set; }
        //EffortValues
        public ICollection<EffortValuesDTO> EffortValues { get; set; }
        //PokemonStats
        public ICollection<PokemonStatsDTO> PokemonStats { get; set; }
        //PokemonAbilities
        public ICollection<PokemonAbilitiesDTO> PokemonAbilities { get; set; }
        //PokemonGameVersion 
        public ICollection<PokemonGameVersionDTO> PokemonGameVersion { get; set; }
        //PokemonEggGroup 
        public ICollection<PokemonEggGroupDTO> PokemonEggGroup { get; set; }
        //PokemonType 
        public ICollection<PokemonTypeDTO> PokemonType { get; set; }
        //PokemonMove
        public ICollection<PokemonMoveDTO> PokemonMove { get; set; }
        //EvolutionChart
        public ICollection<EvolutionChartDTO> EvolutionChart { get; set; }
        public ICollection<EvolutionChartDTO> PreEvolutionChart { get; set; }
    }
}
