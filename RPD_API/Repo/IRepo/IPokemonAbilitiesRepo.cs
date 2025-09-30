using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonAbilitiesRepo
    {
        public Task<bool> AddPokemonAbilities(PostPokemonAbilitiesDTO model, Guid pokeID);
        public Task<bool> DeleteEffortValues(Guid pokeID, Guid abID);
    }
}
