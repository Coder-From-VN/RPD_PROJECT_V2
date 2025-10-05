using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class GrowthRateRepo : IGrowthRateRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public GrowthRateRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GrowthRateDTO?> AddGrowthRate(PostGrowthRateDTO model)
        {
            var existing = await _context.GrowthRate!.SingleOrDefaultAsync(gr => gr.grName == model.grName);

            if (existing != null)
                return null;

            var newGrowthRate = _mapper.Map<GrowthRate>(model);
            _context.GrowthRate!.Add(newGrowthRate);

            var saved = await _context.SaveChangesAsync();
            if (saved > 0)
                return _mapper.Map<GrowthRateDTO?>(newGrowthRate);

            return null;
        }

        public async Task<bool> DeleteGrowthRate(Guid growthRateID)
        {
            var growthRate = _context.GrowthRate!.SingleOrDefault(b => b.growthRateID == growthRateID);
            if (growthRate != null)
            {
                _context.GrowthRate!.Remove(growthRate);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }

        public async Task<List<GrowthRateDTO>> GetAllGrowthRate()
        {
            var growthRate = await _context.GrowthRate.ToListAsync();
            return _mapper.Map<List<GrowthRateDTO>>(growthRate);
        }

        public async Task<GrowthRateDTO> GetGrowthRateById(Guid growthRateID)
        {
            var growthRate = await _context.GrowthRate!.FindAsync(growthRateID);
            return _mapper.Map<GrowthRateDTO>(growthRate);
        }

        public async Task<bool> UpdateGrowthRate(Guid growthRateID, PutGrowthRateDTO model)
        {
            var growthRate = await _context.GrowthRate!.FindAsync(growthRateID);
            if (growthRate != null)
            {
                if (model.grName != "")
                    growthRate.grName = model.grName;
                if (model.grTotalExp != 0)
                    growthRate.grTotalExp = model.grTotalExp;
                _context.GrowthRate!.Update(growthRate);
                var saved = await _context.SaveChangesAsync();
                return saved > 0 ? true : false;
            }
            return false;
        }
    }
}
