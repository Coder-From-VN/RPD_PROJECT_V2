using AutoMapper;
using Google.Apis.Util;
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

        public async Task AddRangeAsync(List<Types> typesList)
        {
            await _context.Types.AddRangeAsync(typesList);
        }

        public async Task<bool> ExistsByIdAsync(Guid typesID)
        {
            return await _context.Types!
                .AnyAsync(t => t.typesID == typesID);
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

        public async Task<List<string>> GetExistingNamesAsync(List<string> names)
        {
            return await _context.Types
                .Where(tn => names.Contains(tn.typesName))
                .Select(tn => tn.typesName)
                .ToListAsync();
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
