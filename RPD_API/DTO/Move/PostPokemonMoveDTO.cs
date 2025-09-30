using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PostPokemonMoveDTO
    {
        public Guid moveID { get; set; }
        public string pmLearnMethod { get; set; }
        public int pmLearnLevel { get; set; }
    }
}
