namespace RPD_WEB.Models
{
    // =======================
    // LIST DTO
    // =======================
    public class PokemonListDto
    {
        public Guid pokeID { get; set; }
        public int pokeNationalNumber { get; set; }
        public string pokeName { get; set; }
    }

    // =======================
    // DETAIL DTO
    // =======================
    public class PokemonDetailDto
    {
        public Guid pokeID { get; set; }

        public int pokeNationalNumber { get; set; }
        public string pokeName { get; set; }
        public string pokeDescription { get; set; }
        public string pokeSpecies { get; set; }

        public int pokeHeight { get; set; }
        public int pokeWidth { get; set; }

        public int pokeCatchRate { get; set; }
        public int pokeBaseFriendship { get; set; }
        public int pokeBaseExp { get; set; }

        public int pokeMaleRate { get; set; }
        public int pokeFemaleRate { get; set; }

        public int pokeEggCycles { get; set; }

        public Guid growthRateID { get; set; }

        public List<ImageLinkDto> imageLink { get; set; } = new();
        public List<PokemonTypeDto> pokemonType { get; set; } = new();
    }

    // =======================
    // NESTED DTO
    // =======================
    public class ImageLinkDto
    {
        public string imgLink { get; set; }
    }

    public class PokemonTypeDto
    {
        public Guid typesID { get; set; }
        public int mainOrSubType { get; set; }
    }

    // =======================
    // CREATE DTO (POST)
    // =======================
    public class PostFullPokemonsDTO
    {
        public int pokeNationalNumber { get; set; }
        public string pokeName { get; set; }
        public string pokeDescription { get; set; }
        public string pokeSpecies { get; set; }

        public int pokeHeight { get; set; }
        public int pokeWidth { get; set; }

        public int pokeCatchRate { get; set; }
        public int pokeBaseFriendship { get; set; }
        public int pokeBaseExp { get; set; }

        public int pokeMaleRate { get; set; }
        public int pokeFemaleRate { get; set; }

        public int pokeEggCycles { get; set; }

        public Guid growthRateID { get; set; }

        public List<ImageLinkDto> imageLink { get; set; } = new();
        public List<PokemonTypeDto> pokemonType { get; set; } = new();
    }

    // =======================
    // UPDATE DTO (PUT)
    // =======================
    public class PutFullPokemonsDTO
    {
        public int pokeNationalNumber { get; set; }
        public string pokeName { get; set; }
        public string pokeDescription { get; set; }
        public string pokeSpecies { get; set; }

        public int pokeHeight { get; set; }
        public int pokeWidth { get; set; }

        public int pokeCatchRate { get; set; }
        public int pokeBaseFriendship { get; set; }
        public int pokeBaseExp { get; set; }

        public int pokeMaleRate { get; set; }
        public int pokeFemaleRate { get; set; }

        public int pokeEggCycles { get; set; }

        public Guid growthRateID { get; set; }

        public List<ImageLinkDto> imageLink { get; set; } = new();
        public List<PokemonTypeDto> pokemonType { get; set; } = new();
    }
}