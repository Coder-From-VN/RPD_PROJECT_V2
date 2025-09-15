namespace RPD_API.Models
{
    public class Move
    {
        public Guid moveID { get; set; }
        public string moveName { get; set; }
        public string moveCategory { get; set; }
        public int movePower { get; set; }
        public double moveAccuracy { get; set; }
        public int movePP { get; set; }
        public int movePriority { get; set; }
        public string moveDescription { get; set; }
        public Guid typeID { get; set; }
        public Type type { get; set; }
    }
}
