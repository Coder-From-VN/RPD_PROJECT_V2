using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PokemonMoveDTO
    {
        public Guid moveID { get; set; }
        public MoveDTO Move { get; set; }
        public string pmLearnMethod { get; set; }
        public int pmLearnLevel { get; set; }
    }
}
