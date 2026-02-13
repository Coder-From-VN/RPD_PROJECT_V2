using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.Caching;
using RPD_API.DTO;
using RPD_API.Middleware.Exceptions;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonApplicationService : IPokemonApplicationService
    {
        private readonly IPokemonService _pokemonService;
        private readonly IPokemonEggGroupService _eggGroupService;
        private readonly IPokemonGameVersionService _gameVersionService;
        private readonly IPokemonTypeService _typeService;
        private readonly IPokemonStatsService _statsService;
        private readonly IPokemonAbilitiesService _abilitiesService;
        private readonly IImageLinkService _imageService;
        private readonly IEffortValuesService _evService;
        private readonly IUnitOfWorkRepo _uow;
        protected readonly IMapper _mapper;

        public PokemonApplicationService(IPokemonService pokemonService,
        IPokemonEggGroupService eggGroupService,
        IPokemonGameVersionService gameVersionService,
        IPokemonTypeService typeService,
        IPokemonStatsService statsService,
        IPokemonAbilitiesService abilitiesService,
        IImageLinkService imageService,
        IEffortValuesService evService,
        IUnitOfWorkRepo uow, 
        IMapper mapper,
        ICacheService cached)
        {
            _pokemonService = pokemonService;
            _eggGroupService = eggGroupService;
            _gameVersionService = gameVersionService;
            _typeService = typeService;
            _statsService = statsService;
            _abilitiesService = abilitiesService;
            _imageService = imageService;
            _evService = evService;
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<PokemonDetailDTO> PostFullPokemons(PostFullPokemonsDTO model)
        {

            var newPokemonDTO = await _pokemonService.PostPokemons(_mapper.Map<PostPokemonDTO>(model));

            Guid newPokemonID = newPokemonDTO.pokeID;
            //Add PokemonEggGroup
            foreach (var pg in model.PokemonEggGroup)
            {
                await _eggGroupService.PokemonEggGroupAddOn(newPokemonID, pg.egID);
            }
            //Add PokemonGameVersion
            foreach (var gv in model.PokemonGameVersion)
            {
                await _gameVersionService.PokemonGameVersionAddOn(newPokemonID, gv);
            }
            //Add PokemonTypes
            foreach (var t in model.PokemonType)
            {
                await _typeService.PokemonTypeAddOn( newPokemonID, t);
            }
            //Add PokemonStats
            foreach (var pst in model.PokemonStats)
            {
                await _statsService.PokemonStatsAddOn(newPokemonID, pst);
            }
            //Add PokemonAbilities
            foreach (var ab in model.PokemonAbilities)
            {
                await _abilitiesService.PokemonAbilitiesAddOn(newPokemonID, ab);
            }
            //Add ImageLink
            foreach (var img in model.ImageLink)
            {
                await _imageService.AddImageLink(img, newPokemonID);
            }
            //Add EffortValues
            foreach (var ev in model.EffortValues)
            {
                await _evService.AddEffortValues(ev, newPokemonID);
            }

            try
            {
                await _uow.SaveAsync(); 
                return await _pokemonService.GetPokemonsById(newPokemonID);
            }
            catch (DbUpdateException)
            {
                throw new BadRequestException("Pokemon đã tồn tại hoặc dữ liệu không hợp lệ");
            }
        }

        public async Task<bool> PutPokemons(Guid pokeId, PutFullPokemonsDTO model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeId);
            if (pokemon == null)
                throw new NotFoundException($"Pokemon with id {pokeId} not found");

            using var tx = await _uow.BeginTransactionAsync();

            try
            {
                await _imageService.UpdateImageLink(pokeId, model.ImageLink);
                await _evService.UpdateEffortValues(pokeId, model.EffortValues);
                await _statsService.UpdatePokemonStats(pokeId, model.PokemonStats);
                await _abilitiesService.UpdatePokemonAbilities(pokeId, model.PokemonAbilities);
                await _eggGroupService.UpdatePokemonEggGroup(pokeId, model.PokemonEggGroup);
                await _typeService.UpdatePokemonType(pokeId, model.PokemonType);

                await _pokemonService.PutPokemons(
                    pokeId,
                    _mapper.Map<PutPokemonDTO>(model)
                );

                await _uow.SaveAsync(); 
                await tx.CommitAsync();

                return true;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteFullPokemons(Guid pokeID)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemon == null)
                return false;

            await _uow.Pokemons.RemoveAsync(pokemon);
            return await _uow.SaveAsync() > 0;
        }
    }
}
