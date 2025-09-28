using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.DTO.StatType;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class StatTypeRepo : IStatTypeRepo
    {

        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public StatTypeRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StatTypeDTO> AddStatType(PostStatTypeDTO model)
        {
            var existing = await _context.StatType!.SingleOrDefaultAsync(st => st.stName == model.stName);
            if (existing != null)
                return null;

            var newStatType = _mapper.Map<StatType>(model);
            _context.StatType!.Add(newStatType);

            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? _mapper.Map<StatTypeDTO>(newStatType) : null;
        }

        public async Task<bool> DeleteStatType(Guid statTypeID)
        {
            var statType = _context.StatType!.SingleOrDefault(b => b.stID == statTypeID);
            if (statType != null)
            {
                _context.StatType!.Remove(statType);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }

        public async Task<List<StatTypeDTO>> GetAllStatType()
        {
            var statType = await _context.StatType.ToListAsync();
            return _mapper.Map<List<StatTypeDTO>>(statType);
        }

        public async Task<StatTypeDTO> GetStatTypeById(Guid statTypeID)
        {
            var statType = await _context.StatType!.FindAsync(statTypeID);
            return _mapper.Map<StatTypeDTO>(statType);
        }

        public async Task<bool> UpdateStatType(Guid statTypeID, StatTypeDTO model)
        {
            if (statTypeID == model.stID)
            {
                var updateType = _mapper.Map<StatType>(model);
                _context.StatType!.Update(updateType);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }

            return false;
        }
    }
}
