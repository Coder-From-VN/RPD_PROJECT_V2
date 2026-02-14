using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IEvolutionChartService : IBaseService
    {
        Task<bool> PostEvolutionChart(PostEvolutionChartDTO model);
        Task<bool> DeleteEvolutionChart(Guid pokeID, Guid prePokeID);
        Task<bool> UpdateEvolutionChart(Guid pokeID, Guid prePokeID,PutEvolutionChartDTO model);
        Task<int> ImportEvolutionChartAsync(IFormFile file);
    }
}
