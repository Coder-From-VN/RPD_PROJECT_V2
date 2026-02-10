using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonGameVersionService : BaseService, IPokemonGameVersionService
    {
        public PokemonGameVersionService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task AddPokemonGameVersion(PostPokemonGameVersionDTO model, Guid pokeID)
        {
            var gameVersionCheck = await _uow.GameVersions.GetByIdAsync(model.gvID);
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (gameVersionCheck == null)
                throw new NotFoundException($"Can't find GameVersions id {model.gvID}");
            if (pokeIdCheck == null)
                throw new NotFoundException($"Can't find Pokemons id {pokeID}");

            var exists = await _uow.PokemonGameVersions.GetLinkAsync(pokeID, model.gvID);
            if (exists != null)
                throw new BadRequestException("GameVersion Alredy Add To this Pokemon");

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
        }

        public async Task<bool> DeletePokemonGameVersion(Guid pokeID, Guid gvID)
        {
            var entry = await _uow.PokemonGameVersions.GetLinkAsync(pokeID, gvID);
            if (entry == null)
                throw new NotFoundException("GameVersion Not Add To Pokemon Yet");

            await _uow.PokemonGameVersions.RemoveAsync(entry);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdatePokemonAbilities(Guid pokeID, ICollection<PutPokemonGameVersionDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Can't find Pokemons id {pokeID}");

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
