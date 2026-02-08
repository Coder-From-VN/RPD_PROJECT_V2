using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonMoveService : BaseService, IPokemonMoveService
    {
        public PokemonMoveService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<bool> AddPokemonMove(PostPokemonMoveDTO model)
        {
            var moveCheck = await _uow.Moves.GetByIdAsync(model.moveID);
            var pokeIdCheck = await _uow.Pokemons.GetByIdAsync(model.pokeID);
            if (moveCheck == null)
                throw new NotFoundException($"Can't find Moves id {model.moveID}");
            if ( pokeIdCheck == null)
                throw new NotFoundException($"Can't find Pokemons id {model.pokeID}");

            var exists = await _uow.PokemonMoves.GetLinkAsync(model.pokeID, model.moveID);
            if (exists != null)
                throw new BadRequestException("Pokemon Alredy Learn This Move");


            PokemonMove newPokemonMove = new PokemonMove
            {
                moveID = model.moveID,
                Move = moveCheck,
                pokeID = model.pokeID,
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
                throw new NotFoundException($"Pokemon id {pokeID} does not exist");

            await _uow.PokemonMoves.RemoveAsync(entry);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<bool> UpdatePokemonMove(Guid pokeID, ICollection<PutPokemonMoveDTO> model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Pokemon id {pokeID} does not exist");

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
