using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PokemonMoveDTO
    {
        public Guid moveID { get; set; }
        //public MoveDTO Move { get; set; }
        //public string pmLearnMethod { get; set; }
        //public int pmLearnLevel { get; set; }
        public string moveName { get; set; }
        public Guid typesID { get; set; }
        public string typesName { get; set; }
        public string moveDamageClass { get; set; }
        public int? movePower { get; set; }
        public int? moveAccuracy { get; set; }
        public int movePP { get; set; }
        public int movePriority { get; set; }
        public string moveDescription { get; set; }

        public string pmLearnMethod { get; set; }
        public int? pmLearnLevel { get; set; }
    }
}
