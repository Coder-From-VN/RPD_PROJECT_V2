using RPD_API.Models;
using System.Linq.Expressions;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonAbilitiesRepo : IBaseRepository
    {
        Task AddAsync(PokemonAbilities model);
        Task<PokemonAbilities?> GetLinkAsync(Guid pokeID, Guid abID);
        Task UpdateAsync(PokemonAbilities model);
        Task RemoveAsync(PokemonAbilities model);
        
        Task RemoveRange(IEnumerable<PokemonAbilities> entities);
        Task AddRangeAsync(List<PokemonAbilities> abilities);
    }
}
