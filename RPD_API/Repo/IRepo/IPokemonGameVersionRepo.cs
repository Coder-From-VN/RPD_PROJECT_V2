using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonGameVersionRepo
    {
        public Task<bool> AddPokemonGameVersion(PostPokemonGameVersionDTO model, Guid pokeID);
        public Task<bool> UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonGameVersionDTO> model);
        public Task<bool> DeletePokemonGameVersion(Guid pokeID, Guid gvID);
    }
}
