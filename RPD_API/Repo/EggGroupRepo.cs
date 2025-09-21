using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class EggGroupRepo : IEggGroupRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public EggGroupRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> AddEggGroup(EggGroupDTO model)
        {
            var newEggGroup = _mapper.Map<EggGroup>(model);
            _context.EggGroup!.Add(newEggGroup);
            await _context.SaveChangesAsync();

            return newEggGroup.egID;
        }

        public async Task DeleteEggGroup(Guid egID)
        {
            var eggGroup = _context.EggGroup!.SingleOrDefault(b => b.egID == egID);
            if (eggGroup != null)
            {
                _context.EggGroup!.Remove(eggGroup);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<EggGroupDTO>> GetAllEggGroup()
        {
            var eggGroup = await _context.EggGroup.ToListAsync();
            return _mapper.Map<List<EggGroupDTO>>(eggGroup);
        }

        public async Task<EggGroupDTO> GetEggGroupById(Guid egID)
        {
            var eggGroup = await _context.EggGroup!.FindAsync(egID);
            return _mapper.Map<EggGroupDTO>(eggGroup);
        }

        public async Task UpdateEggGroup(Guid egID, EggGroupDTO model)
        {
            if (egID == model.egID)
            {
                var updateEggGroup = _mapper.Map<EggGroup>(model);
                _context.EggGroup!.Update(updateEggGroup);
                await _context.SaveChangesAsync();
            }
        }
    }
}
