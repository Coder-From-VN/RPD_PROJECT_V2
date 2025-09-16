namespace RPD_API.Models
{
    public class Type
    {
        public Guid typeID { get; set; }
        public string typeName { get; set; }

        public ICollection<PokemonType> PokemonType { get; set; }

        public ICollection<Move> Move { get; set; }
    }
}
