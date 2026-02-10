using AutoMapper;
using CsvHelper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.Globalization;

namespace RPD_API.Service
{
    public class PokemonMoveService : BaseService, IPokemonMoveService
    {
        public PokemonMoveService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache)
        : base(uow, mapper, cache)
        {
        }

        public async Task<int> AddPokemonMove(PostPokemonMoveListDTO model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(model.pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Can't find Pokemon id {model.pokeID}");

            if (model.moves == null || !model.moves.Any())
                return 0;

            var moveIds = model.moves
                .Select(m => m.moveID)
                .Distinct()
                .ToList();

            var existingMoves = await _uow.Moves.GetByIdsAsync(moveIds);

            if (existingMoves.Count != moveIds.Count)
                throw new NotFoundException("One or more Moves do not exist");

            var existingLinks = await _uow.PokemonMoves
                .GetExistingMovesForPokemonAsync(model.pokeID, moveIds);

            var existingMoveIds = existingLinks
                .Select(x => x.moveID)
                .ToHashSet();

            // 🔧 FIX: only add missing ones
            var newPokemonMoves = model.moves
                .Where(m => !existingMoveIds.Contains(m.moveID))
                .Select(m => new PokemonMove
                {
                    pokeID = model.pokeID,
                    moveID = m.moveID,
                    pmLearnMethod = m.pmLearnMethod,
                    pmLearnLevel = m.pmLearnLevel
                })
                .ToList();

            if (!newPokemonMoves.Any())
                return 0;

            await _uow.PokemonMoves.AddRangeAsync(newPokemonMoves);

            await _uow.SaveAsync();

            return newPokemonMoves.Count;
        }

        public async Task<bool> DeletePokemonMove(Guid pokeID, Guid moveID)
        {
            var entry = await _uow.PokemonMoves.GetLinkAsync(pokeID, moveID);
            if (entry == null)
                throw new NotFoundException($"Pokemon id {pokeID} does not exist");

            await _uow.PokemonMoves.RemoveAsync(entry);
            return await _uow.SaveAsync() > 0;
        }

        public async Task<int> ImportPokemonMoveAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            List<PostPokemonMoveDTO> moveDtos;
            try
            {
                moveDtos = csv.GetRecords<PostPokemonMoveDTO>().ToList();
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Invalid CSV format: {ex}");
            }

            // 🔴 [ADD] Basic validation + normalize
            var normalizedDtos = moveDtos
                .Where(x =>
                    x.pokeID != Guid.Empty &&
                    x.moveID != Guid.Empty &&
                    !string.IsNullOrWhiteSpace(x.pmLearnMethod) &&
                    x.pmLearnLevel > 0
                    )
                .Select(x => new PostPokemonMoveDTO
                {
                    pokeID = x.pokeID,
                    pmLearnLevel = x.pmLearnLevel,
                    moveID = x.moveID,
                    pmLearnMethod = x.pmLearnMethod,
                })
                .GroupBy(x => new { x.pokeID, x.moveID })
                .Select(g => g.First())
                .ToList();

            if (!normalizedDtos.Any())
                return 0;

            // 🔴 [ADD] Check existing evolutions in DB
            var existingPairs = await _uow.PokemonMoves
                .GetExistingPairsAsync(
                    normalizedDtos.Select(x => x.pokeID).ToList(),
                    normalizedDtos.Select(x => x.moveID).ToList()
                );

            var newDtos = normalizedDtos
                .Where(x => !existingPairs.Any(e =>
                    e.pokeID == x.pokeID &&
                    e.moveID == x.moveID))
                .ToList();

            if (!newDtos.Any())
                return 0;

            var pokemonMoves = _mapper.Map<List<PokemonMove>>(newDtos);

            await _uow.PokemonMoves.AddRangeAsync(pokemonMoves);

            return await _uow.SaveAsync() > 0
                ? pokemonMoves.Count
                : throw new BadRequestException("Something went wrong with EvolutionChart import");
        }

        public async Task<bool> UpdatePokemonMove(Guid pokeID, Guid moveID, PutPokemonMoveDTO model)
        {
            var pokemonMove = await _uow.PokemonMoves.GetLinkAsync(pokeID, moveID); 
            if (pokemonMove == null)
                throw new NotFoundException(
                    $"Move {moveID} is not learned by Pokemon {pokeID}"
                ); 

            pokemonMove.pmLearnMethod = model.pmLearnMethod; 
            pokemonMove.pmLearnLevel = model.pmLearnLevel;  

            await _uow.PokemonMoves.UpdateAsync(pokemonMove); 
            return await _uow.SaveAsync() > 0;
        }
    }
}
