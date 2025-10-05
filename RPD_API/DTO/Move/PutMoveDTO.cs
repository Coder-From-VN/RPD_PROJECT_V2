namespace RPD_API.DTO
{
    public class PutMoveDTO
    {
        public string moveName { get; set; }
        public string moveDamageClass { get; set; }
        public int movePower { get; set; }
        public int moveAccuracy { get; set; }
        public int movePP { get; set; }
        public int movePriority { get; set; }
        public string moveDescription { get; set; }
        public Guid typesID { get; set; }
    }
}
