namespace RPD_API.Models
{
    public class PokemonGameVersion
    {
        public Guid gvID { get; set; }
        public Guid pokeID { get; set; }
        public int pgvDexNumber { get; set; }
        public string pgvEntries { get; set; }

        public Pokemons Pokemons { get; set; }
        public GameVersion GameVersion { get; set; }
    }
}
