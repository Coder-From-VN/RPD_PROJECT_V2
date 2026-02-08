using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class TypesService : BaseService, ITypesService
    {
        public TypesService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
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

        public async Task<bool> UpdateTypes(Guid typeID, PostTypesDTO model)
        {
            var type = await _uow.Types.GetByIdAsync(typeID);
            if (type == null)
                throw new NotFoundException($"Types with id {typeID} not found");

            if (model.typesName != "")
                type.typesName = model.typesName;

            await _uow.Types.UpdateAsync(type);
            return await _uow.SaveAsync() > 0;
        }
    }
}
