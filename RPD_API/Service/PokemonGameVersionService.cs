using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonGameVersionService : BaseService, IPokemonGameVersionService
    {
        public PokemonGameVersionService(IUnitOfWorkRepo uow, IMapper mapper)
        : base(uow, mapper)
        {
        }

        public async Task<bool> AddPokemonGameVersion(PostPokemonGameVersionDTO model, Guid pokeID)
        {
            var gameVersionCheck = await _uow.GameVersions.GetByIdAsync(model.gvID);
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (gameVersionCheck == null || pokeIdCheck == null)
                return false;

            var exists = await _uow.PokemonGameVersions.GetLinkAsync(pokeID, model.gvID);
            if (exists != null)
                return false;


            PokemonGameVersion newPokemonGameVersion = new PokemonGameVersion
            {
                gvID = model.gvID,
                GameVersion = gameVersionCheck,
                pokeID = pokeID,
                Pokemons = pokeIdCheck,
                pgvDexNumber = model.pgvDexNumber,
                pgvEntries = model.pgvEntries
            };

            await _uow.PokemonGameVersions.AddAsync(newPokemonGameVersion);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeletePokemonGameVersion(Guid pokeID, Guid gvID)
        {
            var entry = await _uow.PokemonGameVersions.GetLinkAsync(pokeID, gvID);
            if (entry == null)
                return false;

            await _uow.PokemonGameVersions.RemoveAsync(entry);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonGameVersionDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                return false;

            var existingLinks = pokemon.PokemonGameVersion.ToList();
            foreach (var link in existingLinks)
                await _uow.PokemonGameVersions.RemoveAsync(link);

            foreach (var version in pokemon.PokemonGameVersion)
            {
                var dto = model.FirstOrDefault(m => m.gvID == version.gvID);
                if (dto != null)
                {
                    version.pgvDexNumber = dto.pgvDexNumber;
                    version.pgvEntries = dto.pgvEntries;
                }
            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
