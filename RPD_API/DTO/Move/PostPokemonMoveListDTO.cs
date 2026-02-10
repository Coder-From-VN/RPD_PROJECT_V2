using RPD_API.DTO.Move;

namespace RPD_API.DTO
{
    public class PostPokemonMoveListDTO
    {
        public Guid pokeID { get; set; }
        public List<PostPokemonMoveListItem>? moves { get; set; }
    }
}
