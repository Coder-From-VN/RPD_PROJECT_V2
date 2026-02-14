using RPD_API.Models;

namespace RPD_API.DTO
{
    public class PokemonTypeDTO
    {
        //public Guid typesID { get; set; }
        //public TypesDTO Types { get; set; }
        //public int MainOrSubType { get; set; }
        public Guid typesID { get; set; }
        public string typesName { get; set; }
        public int MainOrSubType { get; set; }
    }
}
