using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonGameVersionRepo : IBaseRepository
    {
        public Task AddAsync(PokemonGameVersion model);
        public Task<PokemonGameVersion?> GetLinkAsync(Guid pokeID, Guid gvID);
        public Task UpdateAsync(PokemonGameVersion model);
        public Task RemoveAsync(PokemonGameVersion model);

        Task AddRangeAsync(List<PokemonGameVersion> pokeGVs);
        Task RemoveRange(IEnumerable<PokemonGameVersion> entities);
    }
}
