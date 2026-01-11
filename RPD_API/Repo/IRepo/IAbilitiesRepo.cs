using RPD_API.DTO;
using RPD_API.Models;

namespace RPD_API.Repo.IRepo
{
    public interface IAbilitiesRepo : IBaseRepository
    {
        public Task<List<Abilities>> GetAllAbilities();
        public Task<Abilities> GetAbilitiesById(Guid abID);
        public Task PostAbilities(Abilities model);
        public Task PutAbilities(Guid abID, Abilities model);
        public Task DeleteAbilities(Abilities model);

        public Task<Abilities?> FindAbilitiesById(Guid abID);
        public Task<bool> CheckAbilitiesExistsByName(string abName);
    }
}
