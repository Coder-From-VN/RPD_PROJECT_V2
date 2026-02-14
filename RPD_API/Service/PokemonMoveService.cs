using AutoMapper;
using CsvHelper;
using Microsoft.Extensions.Caching.Distributed;
using RPD_API.Caching;
using RPD_API.DTO;
using RPD_API.DTO.Move;
using RPD_API.Middleware.Exceptions;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;
using System.Globalization;

namespace RPD_API.Service
{
    public class PokemonMoveService : BaseService, IPokemonMoveService
    {
        public PokemonMoveService(IUnitOfWorkRepo uow, IMapper mapper, IDistributedCache cache, ICacheService cached)
        : base(uow, mapper, cache,cached)
        {
        }
        
        public async Task<int> AddPokemonMove(Guid pokeID, List<PostPokemonMoveListItem> model)
        {
            var pokemon = await _uow.Pokemons.GetPokemonWithMovesAsync(pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Can't find Pokemon id {pokeID}");

            if (model == null || !model.Any())
                return 0;

            var moveIds = model
                .Select(m => m.moveID)
                .Distinct()
                .ToList();

            var existingMoves = await _uow.Moves.GetByIdsAsync(moveIds);

            if (existingMoves.Count != moveIds.Count)
                throw new NotFoundException("One or more Moves do not exist");

            var existingSet = pokemon.PokemonMove
                .Select(x => x.moveID)
                .ToHashSet();

            var newDtos = model
                .Where(x => !existingSet.Contains(x.moveID))
                .ToList();

            if (!newDtos.Any())
                return 0;

            var newLinks = _mapper.Map<List<PokemonMove>>(newDtos);

            // Assign foreign key manually
            foreach (var link in newLinks)
            {
                link.pokeID = pokeID;
            }

            await _uow.PokemonMoves.AddRangeAsync(newLinks);

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return newLinks.Count;
        }

        public async Task<bool> DeletePokemonMove(Guid pokeID, Guid moveID)
        {
            var entry = await _uow.PokemonMoves.GetLinkAsync(pokeID, moveID);
            if (entry == null)
                throw new NotFoundException($"Pokemon id {pokeID} does not exist");

            await _uow.PokemonMoves.RemoveAsync(entry);
            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }

        public async Task<int> ImportPokemonMoveAsync(Guid pokeID,IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new BadRequestException("File is empty");

            var pokemon = await _uow.Pokemons.GetPokemonWithMovesAsync(pokeID);
            if (pokemon == null)
                throw new NotFoundException($"Can't find Pokemon id {pokeID}");

            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, CultureInfo.InvariantCulture);

            List<PostPokemonMoveListItem> moveDtos;
            try
            {
                moveDtos = csv.GetRecords<PostPokemonMoveListItem>().ToList();
            }
            catch (Exception ex)
            {
                throw new BadRequestException($"Invalid CSV format: {ex.Message}");
            }

            var normalizedDtos = moveDtos
                .Where(x =>
                    x.moveID != Guid.Empty &&
                    !string.IsNullOrWhiteSpace(x.pmLearnMethod) &&
                    x.pmLearnLevel >= 0
                )
                .GroupBy(x => x.moveID)
                .Select(g => g.First())
                .ToList();

            if (!normalizedDtos.Any())
                return 0;

            var moveIds = normalizedDtos.Select(x => x.moveID).ToList();
            var existingMoves = await _uow.Moves.GetByIdsAsync(moveIds);

            if (existingMoves.Count != moveIds.Count)
                throw new NotFoundException("One or more Moves do not exist");

            // Check already linked moves
            var existingSet = pokemon.PokemonMove
                .Select(x => x.moveID)
                .ToHashSet();

            var newDtos = normalizedDtos
                .Where(x => !existingSet.Contains(x.moveID))
                .ToList();

            if (!newDtos.Any())
                return 0;

            // Map
            var pokemonMoves = _mapper.Map<List<PokemonMove>>(newDtos);

            // Assign FK
            foreach (var move in pokemonMoves)
            {
                move.pokeID = pokeID;
            }

            await _uow.PokemonMoves.AddRangeAsync(pokemonMoves);

            var saved = await _uow.SaveAsync() > 0;

            if (!saved)
                throw new BadRequestException("Something went wrong with MOVE import");

            await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");

            return pokemonMoves.Count;
        }

        public async Task<bool> UpdatePokemonMove(Guid pokeID, Guid moveID, PutPokemonMoveDTO model)
        {
            var pokemonMove = await _uow.PokemonMoves.GetLinkAsync(pokeID, moveID); 
            if (pokemonMove == null)
                throw new NotFoundException($"Move {moveID} is not learned by Pokemon {pokeID}");

            _mapper.Map(model, pokemonMove);

            //await _uow.PokemonMoves.UpdateAsync(pokemonMove); 
            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cache.RemoveAsync($"Pokemons:pokeid:{pokeID}");
            }

            return saved;
        }
    }
}
