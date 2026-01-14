using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonStatsRepo : IBaseRepository
    {
        public Task AddAsync(PokemonStats model);
        public Task<PokemonStats?> GetLinkAsync(Guid pokeID, Guid stID);
        public Task UpdateAsync(PokemonStats model);
        public Task RemoveAsync(PokemonStats model);
    }
}
