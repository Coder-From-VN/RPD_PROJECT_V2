using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IEvolutionChartRepo
    {
        public Task<bool> PostEvolutionChart(PostEvolutionChartDTO model);
        public Task<bool> DeleteEvolutionChart(Guid pokeID, Guid prePokeID);
    }
}
