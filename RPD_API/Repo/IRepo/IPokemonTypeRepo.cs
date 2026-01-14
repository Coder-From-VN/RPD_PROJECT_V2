using RPD_API.DTO;
using RPD_API.DTO.Types;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonTypeRepo : IBaseRepository
    {
        public Task AddAsync(PokemonType model);
        public Task<PokemonType?> GetLinkAsync(Guid pokeID, Guid typesID);
        public Task UpdateAsync(PokemonType model);
        public Task RemoveAsync(PokemonType model);
    }
}
