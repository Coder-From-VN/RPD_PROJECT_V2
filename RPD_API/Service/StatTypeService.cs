using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class StatTypeService : BaseService, IStatTypeService
    {
        public StatTypeService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<StatTypeDTO?> AddStatType(PostStatTypeDTO model)
        {
            if (await _uow.StatTypes.ExistsByNameAsync(model.stName))
                return null;

            var newStatType = _mapper.Map<StatType>(model);
            await _uow.StatTypes.AddAsync(newStatType);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<StatTypeDTO>(newStatType) : null;
        }

        public async Task<bool> DeleteStatType(Guid statTypeID)
        {
            var statType = await _uow.StatTypes.GetByIdAsync(statTypeID);
            if (statType == null)
                return false;

            await _uow.StatTypes.RemoveAsync(statType);
            return await _uow.SaveAsync() > 0;


        }

        public async Task<List<StatTypeDTO>> GetAllStatType()
        {
            var statType = await _uow.StatTypes.GetAllAsync();
            return _mapper.Map<List<StatTypeDTO>>(statType);
        }

        public async Task<StatTypeDTO> GetStatTypeById(Guid statTypeID)
        {
            var statType = await _uow.StatTypes.GetByIdAsync(statTypeID);
            return _mapper.Map<StatTypeDTO>(statType);
        }

        public async Task<bool> UpdateStatType(Guid statTypeID, PostStatTypeDTO model)
        {
            var statType = await _uow.StatTypes.GetByIdAsync(statTypeID);
            if (statType == null)
                return false;

            if (!string.IsNullOrWhiteSpace(model.stName))
                statType.stName = model.stName;

            await _uow.StatTypes.UpdateAsync(statType);
            return await _uow.SaveAsync() > 0;
        }
    }
}
