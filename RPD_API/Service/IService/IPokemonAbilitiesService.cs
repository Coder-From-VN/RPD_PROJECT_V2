using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Service.IService
{
    public interface IPokemonAbilitiesService : IBaseRepository
    {
        Task PokemonAbilitiesAddOn(Guid pokeID, PostPokemonAbilitiesDTO model);
        Task<bool> AddPokemonAbilities(Guid pokeID,PostPokemonAbilitiesDTO model);
        Task<bool> UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonAbilitiesDTO> model);
        Task<bool> DeletePokemonAbilities(Guid pokeID, Guid abID);
    }
}
