using RPD_API.DTO;

namespace RPD_API.Service.IService
{
    public interface IEffortValuesService : IBaseService
    {
        Task EffortValuesAddOn(Guid pokeID, PostPokemonsEffortValuesDTO model);
        Task<bool> PostEffortValues(Guid pokeID,PostPokemonsEffortValuesDTO model);
        Task<bool> UpdateEffortValues(Guid pokeID, ICollection<PutEffortValuesDTO> model);
        Task<bool> DeleteEffortValues(Guid pokeID, Guid evID);
    }
}
