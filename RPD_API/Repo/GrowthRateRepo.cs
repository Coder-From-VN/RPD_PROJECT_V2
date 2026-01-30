using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Pagination;
using RPD_API.Repo.IRepo;

namespace RPD_API.Repo
{
    public class GrowthRateRepo : BaseRepository<GrowthRate>, IGrowthRateRepo
    {
        public GrowthRateRepo(rpdDbContext context) : base(context)
        {
        }

        public async Task AddAsync(GrowthRate model)
        {
            await _context.GrowthRate.AddAsync(model);
        }

        public async Task<PagedResult<GrowthRate>> GetAllAsync(QueryParams queryParams)
        {
            var query = _context.GrowthRate!
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.Search))
            {
                var search = queryParams.Search.ToLower();
                query = query.Where(a =>
                    a.grName.ToLower().Contains(search)
                    );
            }

            query = queryParams.SortBy?.ToLower() switch
            {
                "grName" => queryParams.SortOrder == "desc"
                    ? query.OrderByDescending(a => a.grName)
                    : query.OrderBy(a => a.grName),

                _ => query.OrderBy(a => a.grName)
            };

            return await ToPagedResultAsync(query, queryParams);
        }

        public async Task<GrowthRate?> GetByIdAsync(Guid growthRateID)
        {
            return await _context.GrowthRate!
                .FirstOrDefaultAsync(gr => gr.growthRateID == growthRateID);
        }

        public Task UpdateAsync(GrowthRate model)
        {
            _context.GrowthRate!.Update(model);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(GrowthRate model)
        {
            _context.GrowthRate.Remove(model);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsByNameAsync(string grName)
        {
            return await _context.GrowthRate!
                .AnyAsync(gr => gr.grName == grName);
        }
    }
}
