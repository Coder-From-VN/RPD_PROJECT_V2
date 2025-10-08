namespace RPD_API.DTO
{
    public class PutPokemonDTO
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
    }
}
