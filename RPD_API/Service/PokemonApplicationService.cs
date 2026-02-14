using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
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
        protected readonly IDistributedCache _cache;
        protected readonly ICacheService _cached;

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
        ICacheService cached,
        IDistributedCache cache)
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
            _cache = cache;
            _cached = cached;
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
                await _imageService.ImageLinkAddOn(newPokemonID, img);
            }
            //Add EffortValues
            foreach (var ev in model.EffortValues)
            {
                await _evService.EffortValuesAddOn(newPokemonID, ev);
            }

            var saved = await _uow.SaveAsync() > 0;
            if (saved)
            {
                await _cached.RemoveByPrefixAsync($"Pokemons:all:page:");
            }
            else
            {
                throw new BadRequestException("Pokemon đã tồn tại hoặc dữ liệu không hợp lệ");
            }

            return await _pokemonService.GetPokemonsById(newPokemonID);

        }

        public async Task<bool> PutFullPokemons(Guid pokeId, PutFullPokemonsDTO model)
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

                var saved = await _uow.SaveAsync() > 0;
                if (saved)
                {
                    await tx.CommitAsync();
                    await _cache.RemoveAsync($"Pokemons:pokeid:{pokeId}");
                    await _cached.RemoveByPrefixAsync($"Pokemons:all:page:");
                }

                return saved;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

    }
}
