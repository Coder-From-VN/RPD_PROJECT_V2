using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
using System.Xml.Linq;

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

        public async Task<EggGroupDTO?> AddEggGroup(PostEggGroupDTO model)
        {
            var existing = await _context.EggGroup!.SingleOrDefaultAsync(b => b.egName == model.egName);

            if (existing != null)
                return null;

            var newEggGroup = _mapper.Map<EggGroup>(model);
            _context.EggGroup!.Add(newEggGroup);

            var saved = await _context.SaveChangesAsync();
            if (saved > 0)
                return _mapper.Map<EggGroupDTO?>(newEggGroup);

            return null;
        }

        public async Task<bool> DeleteEggGroup(Guid egID)
        {
            var eggGroup = _context.EggGroup!.SingleOrDefault(b => b.egID == egID);
            if (eggGroup != null)
            {
                _context.EggGroup!.Remove(eggGroup);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
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

        public async Task<bool> UpdateEggGroup(Guid egID, PutEggGroupDTO model)
        {
            var eggGroup = await _context.EggGroup!.FindAsync(egID);

            if (eggGroup != null)
            {
                if (model.egName != "")
                    eggGroup.egName = model.egName;
                _context.EggGroup!.Update(eggGroup);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }
    }
}
