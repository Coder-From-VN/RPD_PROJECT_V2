using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonMoveService : BaseService, IPokemonMoveService
    {
        public PokemonMoveService(IUnitOfWorkRepo uow, IMapper mapper)
        : base(uow, mapper)
        {
        }

        public async Task<bool> AddPokemonMove(PostPokemonMoveDTO model, Guid pokeID)
        {
            var moveCheck = await _uow.Moves.GetByIdAsync(model.moveID);
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (moveCheck == null || pokeIdCheck == null)
                return false;

            var exists = await _uow.PokemonMoves.GetLinkAsync(pokeID, model.moveID);
            if (exists != null)
                return false;


            PokemonMove newPokemonMove = new PokemonMove
            {
                moveID = model.moveID,
                Move = moveCheck,
                pokeID = pokeID,
                Pokemons = pokeIdCheck,
                pmLearnLevel = model.pmLearnLevel,
                pmLearnMethod = model.pmLearnMethod
            };

            await _uow.PokemonMoves.AddAsync(newPokemonMove);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> DeletePokemonMove(Guid pokeID, Guid moveID)
        {
            var entry = await _uow.PokemonMoves.GetLinkAsync(pokeID, moveID);
            if (entry == null)
                return false;

            await _uow.PokemonMoves.RemoveAsync(entry);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdatePokemonMove(Guid pokeID, ICollection<PutPokemonMoveDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                return false;

            var existingLinks = pokemon.PokemonMove.ToList();
            foreach (var link in existingLinks)
                await _uow.PokemonMoves.RemoveAsync(link);

            // Update existing and add new
            foreach (var dto in model)
            {
                var existing = pokemon.PokemonMove.FirstOrDefault(m => m.moveID == dto.moveID);

                if (existing != null)
                {
                    // Update existing move info
                    existing.pmLearnMethod = dto.pmLearnMethod;
                    existing.pmLearnLevel = dto.pmLearnLevel;
                }
                else
                {
                    // Add new move record
                    pokemon.PokemonMove.Add(new PokemonMove
                    {
                        pokeID = pokeID,
                        moveID = dto.moveID,
                        pmLearnMethod = dto.pmLearnMethod,
                        pmLearnLevel = dto.pmLearnLevel
                    });
                }
            }

            return await _uow.SaveAsync() > 0;
        }
    }
}
