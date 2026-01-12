using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IEvolutionChartRepo : IBaseRepository
    {
        public Task AddAsync(EvolutionChart model);
        public Task<List<EvolutionChart>> GetAllAsync();
        public Task<EvolutionChart?> GetByIdAsync(Guid evoID);
        public Task UpdateAsync(EvolutionChart model);
        public Task RemoveAsync(EvolutionChart model);

        public Task<EvolutionChart?> FindAsync(Guid pokeID, Guid prePokeID);
    }
}
