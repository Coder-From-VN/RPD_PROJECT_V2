using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Service.IService
{
    public interface IPokemonAbilitiesService : IBaseRepository
    {
        public Task<bool> AddPokemonAbilities(PostPokemonAbilitiesDTO model, Guid pokeID);
        public Task<bool> UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonAbilitiesDTO> model);
        public Task<bool> DeletePokemonAbilities(Guid pokeID, Guid abID);
    }
}
