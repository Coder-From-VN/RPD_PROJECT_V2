using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class PokemonGameVersionRepo : BaseRepository<PokemonGameVersion>, IPokemonGameVersionRepo
    {
        public PokemonGameVersionRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(PokemonGameVersion model)
        {
            await _context.PokemonGameVersion.AddAsync(model);
        }

        public async Task<PokemonGameVersion?> GetLinkAsync(Guid pokeID, Guid gvID)
        {
            return await _context.PokemonGameVersion
                 .FirstOrDefaultAsync(pg => pg.gvID == gvID && pg.pokeID == pokeID);
        }

        public Task RemoveAsync(PokemonGameVersion model)
        {
            _context.PokemonGameVersion.Remove(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(PokemonGameVersion model)
        {
            _context.PokemonGameVersion!.Update(model);
            return Task.CompletedTask;
        }
    }
}
