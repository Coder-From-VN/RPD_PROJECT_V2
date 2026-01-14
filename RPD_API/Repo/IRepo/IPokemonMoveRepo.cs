using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonMoveRepo : IBaseRepository
    {
        public Task AddAsync(PokemonMove model);
        public Task<PokemonMove?> GetLinkAsync(Guid pokeID, Guid moveID);
        public Task UpdateAsync(PokemonMove model);
        public Task RemoveAsync(PokemonMove model);
    }
}
