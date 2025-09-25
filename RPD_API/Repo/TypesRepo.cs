using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO.GameVersion;
using RPD_API.DTO.Types;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
using System;

namespace RPD_API.Repo
{
    public class TypesRepo : ITypesRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public TypesRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TypesDTO> AddTypes(PostTypesDTO model)
        {
            var existing = await _context.Types!.SingleOrDefaultAsync(b => b.typesName == model.typesName);

            if (existing != null)
                return null;

            var newType = _mapper.Map<Types>(model);
            _context.Types!.Add(newType);
            var saved = await _context.SaveChangesAsync();

            return saved > 0 ? _mapper.Map<TypesDTO>(newType) : null;
        }

        public async Task<bool> DeleteTypes(Guid typeID)
        {
            var type = _context.Types!.SingleOrDefault(b => b.typesID == typeID);
            if (type != null)
            {
                _context.Types!.Remove(type);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }

        public async Task<List<TypesDTO>> GetAllTypes()
        {
            var type = await _context.Types.ToListAsync();
            return _mapper.Map<List<TypesDTO>>(type);
        }

        public async Task<TypesDTO> GetTypesById(Guid typeID)
        {
            var type = await _context.Types!.FindAsync(typeID);
            return _mapper.Map<TypesDTO>(type);
        }

        public async Task<bool> UpdateTypes(Guid typeID, TypesDTO model)
        {
            if (typeID == model.typesID)
            {
                var updateType = _mapper.Map<Models.Types>(model);
                _context.Types!.Update(updateType);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }
    }
}
