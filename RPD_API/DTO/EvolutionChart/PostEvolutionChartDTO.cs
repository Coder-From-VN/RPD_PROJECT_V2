namespace RPD_API.DTO
{
    public class PostEvolutionChartDTO
    {
        public Guid pokeID { get; set; }
        public Guid prePokeID { get; set; }
        public string evoCondition { get; set; }
    }
}
