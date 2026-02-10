using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IEvolutionChartService : IBaseService
    {
        public Task<bool> PostEvolutionChart(PostEvolutionChartDTO model);
        public Task<bool> DeleteEvolutionChart(Guid pokeID, Guid prePokeID);
        Task<int> ImportEvolutionChartAsync(IFormFile file);
    }
}
