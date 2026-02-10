namespace RPD_API.DTO.Move
{
    public class PostPokemonMoveListItem
    {
        public Guid moveID { get; set; }
        public string? pmLearnMethod { get; set; }
        public int pmLearnLevel { get; set; }
    }
}
