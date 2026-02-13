using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IPokemonStatsService : IBaseService
    {
        Task PokemonStatsAddOn(Guid pokeID, PostPokemonStatsDTO model);
        Task<bool> AddPokemonStats(Guid pokeID, PostPokemonStatsDTO model);
        Task<bool> DeletePokemonStats(Guid pokeID, Guid stID);
        Task<bool> UpdatePokemonStats(Guid pokeID, ICollection<PutPokemonStatsDTO> model);
    }
}
