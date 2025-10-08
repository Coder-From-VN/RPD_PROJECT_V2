using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonAbilitiesRepo
    {
        public Task<bool> AddPokemonAbilities(PostPokemonAbilitiesDTO model, Guid pokeID);
        public Task<bool> UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonAbilitiesDTO> model);
        public Task<bool> DeletePokemonAbilities(Guid pokeID, Guid abID);
    }
}
