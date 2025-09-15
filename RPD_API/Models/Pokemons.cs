using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPD_API.Models
{
    public class Pokemons
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid pokeID { get; set; }
        [Required]
        public int pokeNationalNumber { get; set; }
        [Required, MaxLength(100)]
        public string pokeName { get; set; }
        [Required, MaxLength(500)]
        public string pokeDescription { get; set; }
        [Required, MaxLength(100)]
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
        //growthRateID
        public GrowthRate GrowthRate { get; set; }
        //Image 
        public ICollection<ImageLink> Images { get; set; }
        //EffortValues

        //PokemonStats

        //PokemonAbilities
        public ICollection<PokemonAbilities> pokemonAbilities { get; set; }
        //PokemonGameVersion 

        //PokemonEggGroup 

        //PokemonType 

        //PokemonMove

        //EvolutionChart

    }
}
