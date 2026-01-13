using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
using System;
using System.Xml.Linq;

namespace RPD_API.Repo
{
    public class TypesRepo : BaseRepository<Types>, ITypesRepo
    {
        public TypesRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Types model)
        {
            await _context.Types.AddAsync(model);
        }

        public async Task<bool> ExistsByNameAsync(string typesName)
        {
            return await _context.Types!
                .AnyAsync(t => t.typesName == typesName);
        }

        public async Task<List<Types>> GetAllAsync()
        {
            return await _context.Types!.AsNoTracking().ToListAsync();
        }

        public async Task<Types?> GetByIdAsync(Guid typesID)
        {
            return await _context.Types!
                .FirstOrDefaultAsync(t => t.typesID == typesID);
        }

        public Task RemoveAsync(Types model)
        {
            _context.Types.Remove(model);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Types model)
        {
            _context.Types!.Update(model);
            return Task.CompletedTask;
        }
    }
}
