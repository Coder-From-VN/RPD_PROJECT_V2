using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO.EffortValues;
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

        public async Task<Guid> AddEffortValues(EffortValuesDTO model)
        {
            var newEffortValues = _mapper.Map<EffortValues>(model);
            _context.EffortValues!.Add(newEffortValues);
            await _context.SaveChangesAsync();

            return newEffortValues.evID;
        }

        public async Task DeleteEffortValues(Guid evID)
        {
            var effortValues = _context.EffortValues!.SingleOrDefault(b => b.evID == evID);
            if (effortValues != null)
            {
                _context.EffortValues!.Remove(effortValues);
                await _context.SaveChangesAsync();
            }
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

        public async Task UpdateEffortValues(Guid evID, EffortValuesDTO model)
        {
            if (evID == model.evID)
            {
                var updateEffortValues = _mapper.Map<EffortValues>(model);
                _context.EffortValues!.Update(updateEffortValues);
                await _context.SaveChangesAsync();
            }
        }
    }
}
