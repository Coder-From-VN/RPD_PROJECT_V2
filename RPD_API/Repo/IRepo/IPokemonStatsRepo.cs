using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IPokemonStatsRepo
    {
        public Task<bool> AddPokemonStats(PostPokemonStatsDTO model, Guid pokeID);
        public Task<bool> DeletePokemonStats(Guid pokeID, Guid stID);
        public Task<bool> UpdatePokemonStats(Guid pokeID, ICollection<PutPokemonStatsDTO> model);
    }
}
