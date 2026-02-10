using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IEffortValuesService : IBaseService
    {
        Task AddEffortValues(PostPokemonsEffortValuesDTO model, Guid pokeID);
        Task UpdateEffortValues(Guid pokeID, ICollection<PutEffortValuesDTO> model);
        //Task<bool> DeleteEffortValues(Guid evID);
    }
}
