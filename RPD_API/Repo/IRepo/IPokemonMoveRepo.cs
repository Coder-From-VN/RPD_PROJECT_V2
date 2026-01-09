using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonMoveRepo : IBaseRepository
    {
        public Task<bool> AddPokemonMove(PostPokemonMoveDTO model, Guid pokeID);
        public Task<bool> UpdatePokemonMove(Guid pokeID, ICollection<PutPokemonMoveDTO> model);
        public Task<bool> DeletePokemonMove(Guid pokeID, Guid moveID);
    }
}
