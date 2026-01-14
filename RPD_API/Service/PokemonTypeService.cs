using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO.Types;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonTypeService : BaseService, IPokemonTypeService
    {
        public PokemonTypeService(IUnitOfWorkRepo uow, IMapper mapper)
        : base(uow, mapper)
        {
        }

        public async Task<bool> AddPokemonType(Guid typesID, Guid pokeID)
        {
            var typesCheck = await _uow.Types.GetByIdAsync(typesID);
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (typesCheck == null || pokeIdCheck == null)
                return false;

            var exists = await _uow.PokemonTypes.GetLinkAsync(pokeID, typesID);
            if (exists != null)
                return false;


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
                return false;

            await _uow.PokemonTypes.RemoveAsync(entry);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdatePokemonType(Guid pokeID, ICollection<PutPokemonTypeDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                return false;

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
