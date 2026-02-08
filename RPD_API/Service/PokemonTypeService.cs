using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO.Types;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonTypeService : BaseService, IPokemonTypeService
    {
        public PokemonTypeService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<bool> AddPokemonType(Guid typesID, Guid pokeID)
        {
            var typesCheck = await _uow.Types.GetByIdAsync(typesID);
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (typesCheck == null)
                throw new NotFoundException($"Type with ID {typesID} Not Found");
            if (pokeIdCheck == null)
                throw new NotFoundException($"Pokemons with ID {pokeID} Not Found");

            var exists = await _uow.PokemonTypes.GetLinkAsync(pokeID, typesID);
            if (exists != null)
                throw new BadRequestException("Pokemon Alredy Have This Type");

            PokemonType newPokemonType = new PokemonType
            {
                typesID = typesID,
                pokeID = pokeID,
                Pokemons = pokeIdCheck,
                Types = typesCheck
            };

            await _uow.PokemonTypes.AddAsync(newPokemonType);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeletePokemonType(Guid typesID, Guid pokeID)
        {
            var entry = await _uow.PokemonTypes.GetLinkAsync(pokeID, typesID);
            if (entry == null)
                throw new NotFoundException("Pokemon Don't Have This Type");

            await _uow.PokemonTypes.RemoveAsync(entry);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdatePokemonType(Guid pokeID, ICollection<PutPokemonTypeDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Pokemons with ID {pokeID} Not Found");

            var existingLinks = pokemon.PokemonType.ToList();
            foreach (var link in existingLinks)
                await _uow.PokemonTypes.RemoveAsync(link);

            // Add new types that aren’t already linked
            foreach (var dto in model)
            {
                var existing = pokemon.PokemonType.FirstOrDefault(t => t.typesID == dto.typesID);
                if (existing == null)
                {
                    pokemon.PokemonType.Add(new PokemonType
                    {
                        pokeID = pokeID,
                        typesID = dto.typesID
                    });
                }
            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
