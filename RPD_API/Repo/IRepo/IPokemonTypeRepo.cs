using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonTypeRepo
    {
        public Task<bool> AddPokemonType(Guid typesID, Guid pokeID);
        public Task<bool> DeletePokemonType(Guid typesID, Guid pokeID);
    }
}
