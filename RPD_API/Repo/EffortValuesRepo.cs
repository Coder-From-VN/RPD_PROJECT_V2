using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class EffortValuesRepo : IEffortValuesRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public EffortValuesRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EffortValuesDTO> AddEffortValues(PostEffortValuesDTO model)
        {
            var existing = await _context.EffortValues!.SingleOrDefaultAsync(b => b.evStatName == model.evStatName);

            if (existing != null)
                return null;

            var newEffortValues = _mapper.Map<EffortValues>(model);
            _context.EffortValues!.Add(newEffortValues);

            var saved = await _context.SaveChangesAsync();
            if (saved > 0)
                return _mapper.Map<EffortValuesDTO?>(newEffortValues);

            return null;
        }

        public async Task<bool> DeleteEffortValues(Guid evID)
        {
            var effortValues = _context.EffortValues!.SingleOrDefault(b => b.evID == evID);
            if (effortValues != null)
            {
                _context.EffortValues!.Remove(effortValues);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }

        public async Task<List<EffortValuesDTO>> GetAllEffortValues()
        {
            var effortValues = await _context.EffortValues.ToListAsync();
            return _mapper.Map<List<EffortValuesDTO>>(effortValues);
        }

        public async Task<EffortValuesDTO> GetEffortValuesById(Guid evID)
        {
            var effortValues = await _context.EffortValues!.FindAsync(evID);
            return _mapper.Map<EffortValuesDTO>(effortValues);
        }

        public async Task<bool> UpdateEffortValues(Guid evID, EffortValuesDTO model)
        {
            if (evID == model.evID)
            {
                var updateEffortValues = _mapper.Map<EffortValues>(model);
                _context.EffortValues!.Update(updateEffortValues);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }
    }
}
