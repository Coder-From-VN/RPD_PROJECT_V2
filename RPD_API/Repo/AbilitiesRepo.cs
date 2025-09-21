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

        public async Task<Guid> AddAbilities(AbilitiesDTO model)
        {
            var newAbilities = _mapper.Map<Abilities>(model);
            _context.Abilities!.Add(newAbilities);
            await _context.SaveChangesAsync();

            return newAbilities.abID;
        }

        public async Task DeleteAbilities(Guid abID)
        {
            var abilities = _context.Abilities!.SingleOrDefault(b => b.abID == abID);
            if (abilities != null)
            {
                _context.Abilities!.Remove(abilities);
                await _context.SaveChangesAsync();
            }
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

        public async Task UpdateAbilities(Guid abID, AbilitiesDTO model)
        {
            if (abID == model.abID)
            {
                var updateAbilities = _mapper.Map<Abilities>(model);
                _context.Abilities!.Update(updateAbilities);
                await _context.SaveChangesAsync();
            }
        }
    }
}
