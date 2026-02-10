using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IPokemonStatsService : IBaseService
    {
        Task AddPokemonStats(PostPokemonStatsDTO model, Guid pokeID);
        //Task<bool> DeletePokemonStats(Guid pokeID, Guid stID);
        Task UpdatePokemonStats(Guid pokeID, ICollection<PutPokemonStatsDTO> model);
    }
}
