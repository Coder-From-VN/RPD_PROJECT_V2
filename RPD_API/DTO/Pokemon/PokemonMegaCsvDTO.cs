namespace RPD_API.DTO.Pokemon
{
    public class PokemonMegaCsvDTO
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

        public string ImageLinks { get; set; }
        public string EffortValues { get; set; }
        public string PokemonStats { get; set; }
        public string PokemonAbilities { get; set; }
        public string PokemonGameVersion { get; set; }
        public string PokemonEggGroup { get; set; }
        public string PokemonType { get; set; }
    }
}
