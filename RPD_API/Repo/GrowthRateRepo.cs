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

        public async Task<Guid> AddGrowthRate(GrowthRateDTO model)
        {
            var newGrowthRate = _mapper.Map<GrowthRate>(model);
            _context.GrowthRate!.Add(newGrowthRate);
            await _context.SaveChangesAsync();

            return newGrowthRate.growthRateID;
        }

        public async Task DeleteGrowthRate(Guid growthRateID)
        {
            var growthRate = _context.GrowthRate!.SingleOrDefault(b => b.growthRateID == growthRateID);
            if (growthRate != null)
            {
                _context.GrowthRate!.Remove(growthRate);
                await _context.SaveChangesAsync();
            }
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

        public async Task UpdateGrowthRate(Guid growthRateID, GrowthRateDTO model)
        {
            if (growthRateID == model.growthRateID)
            {
                var updateBook = _mapper.Map<GrowthRate>(model);
                _context.GrowthRate!.Update(updateBook);
                await _context.SaveChangesAsync();
            }
        }
    }
}
