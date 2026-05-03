namespace RPD_WEB.Models
{
    public class Move
    {
        public Guid MoveID { get; set; }
        public string MoveName { get; set; }
        public string MoveDamageClass { get; set; }
        public int MovePower { get; set; }
        public int MoveAccuracy { get; set; }
        public int MovePP { get; set; }
        public int MovePriority { get; set; }
        public string MoveDescription { get; set; }
        public string TypesID { get; set; }
        public string TypesName { get; set; }
    }
}
