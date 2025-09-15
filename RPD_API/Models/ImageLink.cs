namespace RPD_API.Models
{
    public class ImageLink
    {
        public Guid imgID { get; set; }
        public string imgLink { get; set; }
        public Guid pokeID { get; set; }
        public Pokemons pokemon { get; set; }
    }
}
