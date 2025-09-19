using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class TypeRepo : ITypeRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public TypeRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> AddType(TypeDTO model)
        {
            var newType = _mapper.Map<Models.Type>(model);
            _context.Type!.Add(newType);
            await _context.SaveChangesAsync();

            return newType.typeID;
        }

        public async Task DeleteType(Guid typeID)
        {
            var type = _context.Type!.SingleOrDefault(b => b.typeID == typeID);
            if (type != null)
            {
                _context.Type!.Remove(type);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TypeDTO>> GetAllType()
        {
            var type = await _context.Type.ToListAsync();
            return _mapper.Map<List<TypeDTO>>(type);
        }

        public async Task<TypeDTO> GetTypeById(Guid typeID)
        {
            var type = await _context.Type!.FindAsync(typeID);
            return _mapper.Map<TypeDTO>(type);
        }

        public async Task UpdateType(Guid typeID, TypeDTO model)
        {
            if (typeID == model.typeID)
            {
                var updateType = _mapper.Map<Models.Type>(model);
                _context.Type!.Update(updateType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
