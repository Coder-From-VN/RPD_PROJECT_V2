using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.DTO.Types;
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
    }
}
