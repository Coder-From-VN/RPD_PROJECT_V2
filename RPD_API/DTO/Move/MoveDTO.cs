using Type = RPD_API.Models.Types;

namespace RPD_API.DTO.Move
{
    public class MoveDTO
    {
        public Guid moveID { get; set; }
        public string moveName { get; set; }
        public string moveDamageClass { get; set; }
        public int movePower { get; set; }
        public double moveAccuracy { get; set; }
        public int movePP { get; set; }
        public int movePriority { get; set; }
        public string moveDescription { get; set; }
        public Guid typesID { get; set; }
        public string TypesName { get; set; }
    }
}
