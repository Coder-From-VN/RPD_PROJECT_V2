using RPD_API.DTO;
using RPD_API.Repo.IRepo;

namespace RPD_API.Service.IService
{
    public interface IPokemonGameVersionService : IBaseRepository
    {
        public Task AddPokemonGameVersion(PostPokemonGameVersionDTO model, Guid pokeID);
        public Task<bool> UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonGameVersionDTO> model);
        public Task<bool> DeletePokemonGameVersion(Guid pokeID, Guid gvID);
    }
}
