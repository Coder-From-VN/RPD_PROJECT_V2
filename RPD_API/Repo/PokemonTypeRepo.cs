using Microsoft.EntityFrameworkCore;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonTypeRepo : BaseRepository<PokemonType>, IPokemonTypeRepo
    {
        public PokemonTypeRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(PokemonType model)
        {
            await _context.PokemonType.AddAsync(model);
        }

        public async Task<PokemonType?> GetLinkAsync(Guid pokeID, Guid typesID)
        {
            return await _context.PokemonType
                .FirstOrDefaultAsync(pt => pt.typesID == typesID && pt.pokeID == pokeID);
        }

        public Task RemoveAsync(PokemonType model)
        {
            _context.PokemonType.Remove(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(PokemonType model)
        {
            _context.PokemonType!.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveRange(IEnumerable<PokemonType> entities)
        {
            _context.PokemonType.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public async Task AddRangeAsync(List<PokemonType> types)
        {
            await _context.PokemonType.AddRangeAsync(types);
        }
    }
}
