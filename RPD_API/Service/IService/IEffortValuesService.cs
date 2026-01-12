using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IEffortValuesService : IBaseService
    {
        //Add some get pokemon by ev here
        public Task<bool> AddEffortValues(PostPokemonsEffortValuesDTO model, Guid pokeID);
        public Task<bool> UpdateEffortValues(Guid pokeID, ICollection<PutEffortValuesDTO> model);
        public Task<bool> DeleteEffortValues(Guid evID);
    }
}
