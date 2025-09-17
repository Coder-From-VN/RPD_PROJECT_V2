using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPD_API.Models
{
    public class Pokemons
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        //growthRate
        public Guid growthRateID { get; set; }
        public GrowthRate GrowthRate { get; set; }
        //Image 
        public ICollection<ImageLink> ImageLink { get; set; }
        //EffortValues
        public ICollection<EffortValues> EffortValues { get; set; }
        //PokemonStats
        public ICollection<PokemonStats> PokemonStats { get; set; }
        //PokemonAbilities
        public ICollection<PokemonAbilities> PokemonAbilities { get; set; }
        //PokemonGameVersion 
        public ICollection<PokemonGameVersion> PokemonGameVersion { get; set; }
        //PokemonEggGroup 
        public ICollection<PokemonEggGroup> PokemonEggGroup { get; set; }
        //PokemonType 
        public ICollection<PokemonType> PokemonType { get; set; }
        //PokemonMove
        public ICollection<PokemonMove> PokemonMove { get; set; }
        //EvolutionChart
        public ICollection<EvolutionChart> EvolutionChart { get; set; }
        public ICollection<EvolutionChart> PreEvolutionChart { get; set; }
    }
}
