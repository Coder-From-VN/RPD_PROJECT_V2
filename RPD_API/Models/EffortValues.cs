namespace RPD_API.Models
{
    public class EffortValues
    {
        public Guid pokeID { get; set; }
        public Guid stID { get; set; }
        public int eValues { get; set; }

        public Pokemons pokemon { get; set; }
        public StatType statType { get; set; }
    }
}
