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

        public async Task<bool> AddEffortValues(PostPokemonsEffortValuesDTO model, Guid pokeID)
        {
            var pokeIdCheck = await _context.Pokemons!
                .SingleOrDefaultAsync(p => p.pokeID == pokeID);
            if (pokeIdCheck == null)
                return false;
            EffortValues newEffortValues = new EffortValues
            {
                evStatName = model.evStatName,
                eValues = model.eValues,
                pokeID = pokeID,
                Pokemons = pokeIdCheck
            };
            _context.EffortValues!.Add(newEffortValues);
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
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

        //public async Task<List<EffortValuesDTO>> GetAllEffortValues()
        //{
        //    var effortValues = await _context.EffortValues.ToListAsync();
        //    return _mapper.Map<List<EffortValuesDTO>>(effortValues);
        //}

        public async Task<bool> UpdateEffortValues(Guid pokeID, ICollection<PutEffortValuesDTO> model)
        {
            var pokemon = await _context.Pokemons
                .Include(ev => ev.EffortValues)
                .FirstOrDefaultAsync(p => p.pokeID == pokeID);

            if (pokemon == null)
                return false;

            foreach (var ev in pokemon.EffortValues)
            {
                var dto = model.FirstOrDefault(m => m.evStatName == ev.evStatName);
                if (dto != null)
                {
                    ev.eValues = dto.eValues;
                    ev.evStatName = dto.evStatName;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
