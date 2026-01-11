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

        public async Task<bool> CheckAbilitiesExistsByName(string abName)
        {
            return await _context.Abilities!
                .AnyAsync(ab => ab.abName == abName);
        }

        public async Task<Abilities?> FindAbilitiesById(Guid abID)
        {
            return await _context.Abilities!
                .FirstOrDefaultAsync(ab => ab.abID == abID);
        }

        public async Task PostAbilities(Abilities model)
        {
            await _context.Abilities.AddAsync(model);
        }

        public Task DeleteAbilities(Abilities model)
        {
            _context.Abilities.Remove(model);
            return Task.CompletedTask;
        }

        public async Task<bool> PutAbilities(Guid abID, PutAbilitiesDTO model)
        {
            var abilities = _context.Abilities!.SingleOrDefault(b => b.abID == abID);
            if (abilities != null)
            {
                if (model.abName != "")
                    abilities.abName = model.abName;
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

        public Task<List<Abilities>> GetAllAbilities()
        {
            throw new NotImplementedException();
        }

        public Task<Abilities> GetAbilitiesById(Guid abID)
        {
            throw new NotImplementedException();
        }

        public Task PutAbilities(Guid abID, Abilities model)
        {
            throw new NotImplementedException();
        }
    }
}
