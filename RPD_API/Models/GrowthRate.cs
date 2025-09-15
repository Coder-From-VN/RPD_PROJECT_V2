namespace RPD_API.Models
{
    public class GrowthRate
    {
        public Guid growthRateID { get; set; }
        public string grName { get; set; }
        public double grTotalExp { get; set; }

        public ICollection<Pokemons> Pokemons { get; set; }
    }
}
