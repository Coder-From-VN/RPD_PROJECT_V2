using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonStatsService : BaseService, IPokemonStatsService
    {
        public PokemonStatsService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<bool> AddPokemonStats(PostPokemonStatsDTO model, Guid pokeID)
        {
            var statsCheck = await _uow.StatTypes.GetByIdAsync(model.stID);
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (statsCheck == null || pokeIdCheck == null)
                return false;

            var exists = await _uow.PokemonStats.GetLinkAsync(pokeID, model.stID);
            if (exists != null)
                return false;


            PokemonStats newPokemonStats = new PokemonStats
            {
                stID = model.stID,
                pokeID = pokeID,
                Pokemons = pokeIdCheck,
                StatType = statsCheck,
                Basevalue = model.Basevalue,
                minValue = model.minValue,
                MaxValue = model.MaxValue

            };

            await _uow.PokemonStats.AddAsync(newPokemonStats);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeletePokemonStats(Guid pokeID, Guid stID)
        {
            var entry = await _uow.PokemonStats.GetLinkAsync(pokeID, stID);
            if (entry == null)
                return false;

            await _uow.PokemonStats.RemoveAsync(entry);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdatePokemonStats(Guid pokeID, ICollection<PutPokemonStatsDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                return false;

            var existingLinks = pokemon.PokemonStats.ToList();
            foreach (var link in existingLinks)
                await _uow.PokemonStats.RemoveAsync(link);

            foreach (var stat in pokemon.PokemonStats)
            {
                var dto = model.FirstOrDefault(m => m.stID == stat.stID);
                if (dto != null)
                {
                    stat.Basevalue = dto.Basevalue;
                    stat.minValue = dto.minValue;
                    stat.MaxValue = dto.MaxValue;
                }
            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
