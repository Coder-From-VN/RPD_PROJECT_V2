using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PostPokemonStatsDTO
    {
        public Guid stID { get; set; }
        public int Basevalue { get; set; }
        public int minValue { get; set; }
        public int MaxValue { get; set; }
    }
}
