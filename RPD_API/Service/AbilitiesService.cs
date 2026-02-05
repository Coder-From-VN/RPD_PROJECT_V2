using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.Text.Json;

namespace RPD_API.Service
{
    public class AbilitiesService : BaseService, IAbilitiesService
    {

        public AbilitiesService(
        IUnitOfWorkRepo uow,
        IMapper mapper,
        IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<AbilitiesDTO?> PostAbilities(PostAbilitiesDTO model)
        {
            if (await _uow.Abilities.ExistsByNameAsync(model.abName))
                return null;

            var newAbilities = _mapper.Map<Abilities>(model);

            await _uow.Abilities.AddAsync(newAbilities);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<AbilitiesDTO?>(newAbilities) : null;
        }

        public async Task<AbilitiesDTO?> GetAbilitiesById(Guid abID)
        {
            var ability = await _uow.Abilities.GetByIdAsync(abID);
            if (ability == null)
                return null;

            var dto = _mapper.Map<AbilitiesDTO>(ability);

            return dto;
        }

        public async Task<PagedResult<AbilitiesDTO>> GetAllAbilities(QueryParams queryParams)
        {
            var cacheKey = $"abilities:all:page:{queryParams.PageNumber}";

            var cached = await _cache.GetStringAsync(cacheKey);

            if (cached != null)
            {
                return JsonSerializer.Deserialize<PagedResult<AbilitiesDTO>>(cached)!;
            }

            var result = await GetPagedAsync<Abilities, AbilitiesDTO>(
                queryParams,
                _uow.Abilities.GetAllAsync
            );

            await _cache.SetStringAsync(
           cacheKey,
           JsonSerializer.Serialize(result),
           new DistributedCacheEntryOptions
           {
               AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3)
           });

            return result;
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
            return await _uow.SaveAsync() > 0;
        }


    }
}
