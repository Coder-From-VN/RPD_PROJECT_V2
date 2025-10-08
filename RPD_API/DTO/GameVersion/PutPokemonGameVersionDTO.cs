namespace RPD_API.DTO
{
    public class PutPokemonGameVersionDTO
    {
        public Guid gvID { get; set; }
        public int pgvDexNumber { get; set; }
        public string pgvEntries { get; set; }
    }
}
