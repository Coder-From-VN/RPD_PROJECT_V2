using AutoMapper;
using CsvHelper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.Globalization;

namespace RPD_API.Service
{
    public class TypesService : BaseService, ITypesService
    {
        public TypesService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache, cached)
        {
        }

        public async Task<TypesDTO?> AddTypes(PostTypesDTO model)
        {
            if (await _uow.Types.ExistsByNameAsync(model.typesName))
                throw new BadRequestException("Types name already exists");

            var newType = _mapper.Map<Types>(model);
            await _uow.Types.AddAsync(newType);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<TypesDTO>(newType) : null;
        }

        public async Task<bool> DeleteTypes(Guid typeID)
        {
            var type = await _uow.Types.GetByIdAsync(typeID);
            if (type == null)
                throw new NotFoundException($"Types with id {typeID} not found");

            await _uow.Types.RemoveAsync(type);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<List<TypesDTO>> GetAllTypes()
        {
            var type = await _uow.Types.GetAllAsync();
            return _mapper.Map<List<TypesDTO>>(type);
        }

        public async Task<TypesDTO> GetTypesById(Guid typeID)
        {
            var type = await _uow.Types.GetByIdAsync(typeID);

            if (type == null)
                throw new NotFoundException($"Types with id {typeID} not found");

            return _mapper.Map<TypesDTO>(type);
        }

        public async Task<int> ImportTypesAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            var typeDtos = csv.GetRecords<PostTypesDTO>().ToList();

            var normalizedDtos = typeDtos
                .Where(x => !string.IsNullOrWhiteSpace(x.typesName))
                .Select(x => new PostTypesDTO
                {
                    typesName = x.typesName,
                })
                .GroupBy(x => x.typesName.ToLower())
                .Select(g => g.First())
                .ToList();

            var names = normalizedDtos
                .Select(x => x.typesName)
                .ToList();

            var existingNames = await _uow.Types
                .GetExistingNamesAsync(names);

            var newDtos = normalizedDtos
                .Where(x => !existingNames.Contains(x.typesName))
                .ToList();

            var types = _mapper.Map<List<Types>>(newDtos);

            if (!types.Any())
                return 0;

            await _uow.Types.AddRangeAsync(types);

            return await _uow.SaveAsync() > 0 ? types.Count : throw new BadRequestException("something worng with abilities list");
        }

        public async Task<bool> UpdateTypes(Guid typeID, PutTypesDTO model)
        {
            var type = await _uow.Types.GetByIdAsync(typeID);
            if (type == null)
                throw new NotFoundException($"Types with id {typeID} not found");

            _mapper.Map(model, type);

            //await _uow.Types.UpdateAsync(type);

            return await _uow.SaveAsync() > 0;
        }
    }
}
