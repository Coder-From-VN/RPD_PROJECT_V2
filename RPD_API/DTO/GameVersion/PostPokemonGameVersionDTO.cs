namespace RPD_API.DTO.GameVersion
{
    public class PostPokemonGameVersionDTO
    {
        public Guid gvID { get; set; }
        public int pgvDexNumber { get; set; }
        public string pgvEntries { get; set; }
    }
}
