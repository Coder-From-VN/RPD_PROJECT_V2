using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class AbilitiesRepo : IAbilitiesRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public AbilitiesRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AbilitiesDTO?> PostAbilities(PostAbilitiesDTO model)
        {
            var existing = await _context.Abilities!.SingleOrDefaultAsync(b => b.abName == model.abName);

            if (existing != null)
                return null;

            var newAbilities = _mapper.Map<Abilities>(model);
            _context.Abilities!.Add(newAbilities);

            var saved = await _context.SaveChangesAsync();
            if (saved > 0)
                return _mapper.Map<AbilitiesDTO?>(newAbilities);

            return null;
        }

        public async Task<bool> DeleteAbilities(Guid abID)
        {
            var abilities = _context.Abilities!.SingleOrDefault(b => b.abID == abID);
            if (abilities != null)
            {
                _context.Abilities!.Remove(abilities);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }

        public async Task<AbilitiesDTO> GetAbilitiesById(Guid abID)
        {
            var abilities = await _context.Abilities!.FindAsync(abID);
            return _mapper.Map<AbilitiesDTO>(abilities);
        }

        public async Task<List<AbilitiesDTO>> GetAllAbilities()
        {
            var abilities = await _context.Abilities.ToListAsync();
            return _mapper.Map<List<AbilitiesDTO>>(abilities);
        }


        public async Task<bool> PutAbilities(Guid abID, PutAbilitiesDTO model)
        {
            var abilities = _context.Abilities!.SingleOrDefault(b => b.abID == abID);
            if (abilities != null)
            {
                if (model.abDescription != "")
                    abilities.abDescription = model.abDescription;
                if (model.abEffect != "")
                    abilities.abEffect = model.abEffect;

                _context.Abilities!.Update(abilities);
                var check = await _context.SaveChangesAsync();
                return check > 0 ? true : false;
            }
            return false;
        }
    }
}
