namespace RPD_API.Repo.IRepo
{
    public interface IPokemonEggGroupRepo
    {
        public Task<bool> AddPokemonEggGroup(Guid egID, Guid pokeID);
        public Task<bool> DeletePokemonEggGroup(Guid egID, Guid pokeID);
    }
}
