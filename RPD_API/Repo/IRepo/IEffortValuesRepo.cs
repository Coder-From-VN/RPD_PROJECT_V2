using RPD_API.DTO;

namespace RPD_API.Repo.IRepo
{
    public interface IEffortValuesRepo
    {
        //public Task<List<EffortValuesDTO>> GetAllEffortValues();
        public Task<bool> AddEffortValues(PostPokemonsEffortValuesDTO model, Guid pokeID);
        public Task<bool> UpdateEffortValues(Guid pokeID, ICollection<PutEffortValuesDTO> model);
        public Task<bool> DeleteEffortValues(Guid evID);
    }
}
