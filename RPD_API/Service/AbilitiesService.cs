using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System;

namespace RPD_API.Service
{
    public class AbilitiesService : BaseService, IAbilitiesService
    {
        public AbilitiesService(IUnitOfWorkRepo uow, IMapper mapper)
        : base(uow, mapper)
        {
        }

        public async Task<AbilitiesDTO> PostAbilities(PostAbilitiesDTO model)
        {
            if (await _uow.Abilities.ExistsByNameAsync(model.abName))
                return null;

            var newAbilities = _mapper.Map<Abilities>(model);

            await _uow.Abilities.AddAsync(newAbilities);

            await _uow.SaveAsync();

            return _mapper.Map<AbilitiesDTO?>(newAbilities);
        }

        public async Task<AbilitiesDTO> GetAbilitiesById(Guid abID)
        {
            var ability = await _uow.Abilities.GetByIdAsync(abID);

            if (ability == null)
                return null;

            return _mapper.Map<AbilitiesDTO>(ability);
        }

        public async Task<List<AbilitiesDTO>> GetAllAbilities()
        {
            var abilities = await _uow.Abilities.GetAllAsync();
            return _mapper.Map<List<AbilitiesDTO>>(abilities);
        }

        public async Task<bool> PutAbilities(Guid abID, PutAbilitiesDTO model)
        {
            var abilities = await _uow.Abilities.GetByIdAsync(abID);
            if (abilities != null)
            {
                if (!string.IsNullOrWhiteSpace(model.abName))
                    abilities.abName = model.abName;

                if (!string.IsNullOrWhiteSpace(model.abDescription))
                    abilities.abDescription = model.abDescription;

                if (!string.IsNullOrWhiteSpace(model.abEffect))
                    abilities.abEffect = model.abEffect;

                await _uow.Abilities.UpdateAsync(abilities);
                return await _uow.SaveAsync() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteAbilities(Guid guid)
        {
            var deleteThisAbilities = await _uow.Abilities.GetByIdAsync(guid);
            if (deleteThisAbilities == null)
                return false;

            await _uow.Abilities.RemoveAsync(deleteThisAbilities);
            return await _uow.SaveAsync() > 0 ? true : false;
        }


    }
}
