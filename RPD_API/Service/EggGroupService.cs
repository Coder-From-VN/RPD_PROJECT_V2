using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class EggGroupService : BaseService, IEggGroupService
    {
        public EggGroupService(IUnitOfWorkRepo uow, IMapper mapper)
        : base(uow, mapper)
        {
        }

        public async Task<EggGroupDTO?> AddEggGroup(PostEggGroupDTO model)
        {
            if (await _uow.EggGroups.ExistsByNameAsync(model.egName))
                return null;

            var newEggGroup = _mapper.Map<EggGroup>(model);
            await _uow.EggGroups.AddAsync(newEggGroup);

            if (await _uow.SaveAsync() > 0)
                return _mapper.Map<EggGroupDTO?>(newEggGroup);
            return null;
        }

        public async Task<bool> DeleteEggGroup(Guid egID)
        {
            var eggGroup = await _uow.EggGroups.GetByIdAsync(egID);

            if (eggGroup == null)
                return false;

            await _uow.EggGroups.RemoveAsync(eggGroup);
            return await _uow.SaveAsync() > 0;
        }

        public Task<PagedResult<EggGroupDTO>> GetAllEggGroup(QueryParams queryParams)
        {
            return GetPagedAsync<EggGroup, EggGroupDTO>(
                queryParams,
                _uow.EggGroups.GetAllAsync
            );
        }

        public async Task<EggGroupDTO?> GetEggGroupById(Guid egID)
        {
            var eggGroup = await _uow.EggGroups.GetByIdAsync(egID);
            return eggGroup == null ? null : _mapper.Map<EggGroupDTO>(eggGroup);
        }

        public async Task<bool> UpdateEggGroup(Guid egID, PutEggGroupDTO model)
        {
            var eggGroup = await _uow.EggGroups.GetByIdAsync(egID);

            if (eggGroup == null)
                return false;

            if (!string.IsNullOrWhiteSpace(model.egName))
                eggGroup.egName = model.egName;

            await _uow.EggGroups.UpdateAsync(eggGroup);

            return await _uow.SaveAsync() > 0;

        }
    }
}
