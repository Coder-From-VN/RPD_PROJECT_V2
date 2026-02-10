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

        Task AddRangeAsync(List<PokemonMove> moveList);
        Task<List<PokemonMove>> GetExistingPairsAsync(List<Guid> pokeIds, List<Guid> moveIDs);
        Task<List<PokemonMove>> GetExistingMovesForPokemonAsync(Guid pokeID, List<Guid> moveIds);
    }
}
