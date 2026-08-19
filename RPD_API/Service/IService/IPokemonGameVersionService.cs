using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IPokemonGameVersionService : IBaseService
    {
        public Task PokemonGameVersionAddOn(Guid pokeID,PostPokemonGameVersionDTO model);
        public Task<bool> PostPokemonGameVersion(Guid pokeID, PostPokemonGameVersionDTO model);
        public Task<bool> UpdatePokemonGameVersion(Guid pokeID, ICollection<PutPokemonGameVersionDTO> model);
        public Task<bool> DeletePokemonGameVersion(Guid pokeID, Guid gvID);
    }
}
