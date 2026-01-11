using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class AbilitiesService : IAbilitiesService
    {
        private readonly IUnitOfWorkRepo _uow;
        private readonly IMapper _mapper;

        public AbilitiesService(IUnitOfWorkRepo uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<AbilitiesDTO> PostAbilities(PostAbilitiesDTO model)
        {
            if (await _uow.Abilities.CheckAbilitiesExistsByName(model.abName))
                return null;

            var newAbilities = _mapper.Map<Abilities>(model);

            await _uow.Abilities.PostAbilities(newAbilities);

            await _uow.SaveAsync();

            return _mapper.Map<AbilitiesDTO?>(newAbilities);
        }

        public async Task<bool> DeleteAbilities(Guid Guid)
        {
            var deleteThisAbilities = await _uow.Abilities.FindAbilitiesById(Guid);
            if (deleteThisAbilities == null)
                return false;

            await _uow.Abilities.DeleteAbilities(deleteThisAbilities);
            return await _uow.SaveAsync() > 0 ? true : false;
        }

        public Task<AbilitiesDTO> GetAbilitiesById(Guid abID)
        {
            throw new NotImplementedException();
        }

        public Task<List<AbilitiesDTO>> GetAllAbilities()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PutAbilities(Guid abID, PutAbilitiesDTO model)
        {
            throw new NotImplementedException();
        }
    }
}
