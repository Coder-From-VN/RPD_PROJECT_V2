using AutoMapper;
using CsvHelper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.Globalization;

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
                throw new BadRequestException("StatTypes name already exists");

            var newStatType = _mapper.Map<StatType>(model);
            await _uow.StatTypes.AddAsync(newStatType);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<StatTypeDTO>(newStatType) : null;
        }

        public async Task<bool> DeleteStatType(Guid statTypeID)
        {
            var statType = await _uow.StatTypes.GetByIdAsync(statTypeID);
            if (statType == null)
                throw new NotFoundException($"StatTypes with id {statTypeID} not found");

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
            if (statType == null)
                throw new NotFoundException($"StatTypes with id {statTypeID} not found");
            return _mapper.Map<StatTypeDTO>(statType);
        }

        public async Task<int> ImportStatTypeAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var stDtos = csv.GetRecords<PostStatTypeDTO>().ToList();

            var normalizedDtos = stDtos
                .Where(x => !string.IsNullOrWhiteSpace(x.stName))
                .Select(x => new PostStatTypeDTO
                {
                    stName = x.stName,
                })
                .GroupBy(x => x.stName.ToLower())
                .Select(g => g.First())
                .ToList();

            var names = normalizedDtos
                .Select(x => x.stName)
                .ToList();

            var existingNames = await _uow.StatTypes
                .GetExistingNamesAsync(names);

            var newDtos = normalizedDtos
                .Where(x => !existingNames.Contains(x.stName))
                .ToList();

            var statTypes = _mapper.Map<List<StatType>>(newDtos);

            if (!statTypes.Any())
                return 0;

            await _uow.StatTypes.AddRangeAsync(statTypes);

            return await _uow.SaveAsync() > 0 ? statTypes.Count : throw new BadRequestException("something worng with abilities list");
        }

        public async Task<bool> UpdateStatType(Guid statTypeID, PostStatTypeDTO model)
        {
            var statType = await _uow.StatTypes.GetByIdAsync(statTypeID);
            if (statType == null)
                throw new NotFoundException($"StatTypes with id {statTypeID} not found");

            if (!string.IsNullOrWhiteSpace(model.stName))
                statType.stName = model.stName;

            await _uow.StatTypes.UpdateAsync(statType);
            return await _uow.SaveAsync() > 0;
        }
    }
}
