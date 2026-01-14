using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Service.IService
{
    public interface IPokemonMoveService : IBaseRepository
    {
        public Task<bool> AddPokemonMove(PostPokemonMoveDTO model, Guid pokeID);
        public Task<bool> UpdatePokemonMove(Guid pokeID, ICollection<PutPokemonMoveDTO> model);
        public Task<bool> DeletePokemonMove(Guid pokeID, Guid moveID);
    }
}
