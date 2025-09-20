using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
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

        public async Task<Guid> AddStatType(StatTypeDTO model)
        {
            var newStatType = _mapper.Map<StatType>(model);
            _context.StatType!.Add(newStatType);
            await _context.SaveChangesAsync();

            return newStatType.stID;
        }

        public async Task DeleteStatType(Guid statTypeID)
        {
            var statType = _context.StatType!.SingleOrDefault(b => b.stID == statTypeID);
            if (statType != null)
            {
                _context.StatType!.Remove(statType);
                await _context.SaveChangesAsync();
            }
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

        public async Task UpdateStatType(Guid statTypeID, StatTypeDTO model)
        {
            if (statTypeID == model.stID)
            {
                var updateType = _mapper.Map<StatType>(model);
                _context.StatType!.Update(updateType);
                await _context.SaveChangesAsync();
            }
        }
    }
}
