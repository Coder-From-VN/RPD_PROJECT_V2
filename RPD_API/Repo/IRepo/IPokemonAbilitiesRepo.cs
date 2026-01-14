using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonAbilitiesRepo : IBaseRepository
    {
        public Task AddAsync(PokemonAbilities model);
        public Task<PokemonAbilities?> GetLinkAsync(Guid pokeID, Guid abID);
        public Task UpdateAsync(PokemonAbilities model);
        public Task RemoveAsync(PokemonAbilities model);
    }
}
