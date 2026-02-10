using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IEvolutionChartRepo : IBaseRepository
    {
        Task AddAsync(EvolutionChart model);
        Task AddRangeAsync(List<EvolutionChart> evoList);
        Task<List<EvolutionChart>> GetAllAsync();
        Task<EvolutionChart?> GetByIdAsync(Guid evoID);
        Task UpdateAsync(EvolutionChart model);
        Task RemoveAsync(EvolutionChart model);

        Task<List<EvolutionChart>> GetExistingPairsAsync(List<Guid> pokeIds, List<Guid> prePokeIds);
        Task<EvolutionChart?> FindAsync(Guid pokeID, Guid prePokeID);
    }
}
