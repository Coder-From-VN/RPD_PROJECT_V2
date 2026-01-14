using AutoMapper;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonApplicationService : IPokemonApplicationService
    {
        private readonly IPokemonService _pokemonService;
        private readonly IPokemonEggGroupService _eggGroupService;
        private readonly IPokemonGameVersionService _gameVersionService;
        private readonly IPokemonMoveService _moveService;
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
        IPokemonMoveService moveService,
        IPokemonTypeService typeService,
        IPokemonStatsService statsService,
        IPokemonAbilitiesService abilitiesService,
        IImageLinkService imageService,
        IEffortValuesService evService,
        IUnitOfWorkRepo uow, IMapper mapper)
        {
            _pokemonService = pokemonService;
            _eggGroupService = eggGroupService;
            _gameVersionService = gameVersionService;
            _moveService = moveService;
            _typeService = typeService;
            _statsService = statsService;
            _abilitiesService = abilitiesService;
            _imageService = imageService;
            _evService = evService;
            _uow = uow;
            _mapper = mapper;
        }


        public async Task<PokemonDetailDTO> PostPokemons(PostFullPokemonsDTO model)
        {
            if (await _uow.Pokemons.ExistsByNationalNumberAsync(model.pokeNationalNumber))
                return null;

            var newPokemonDTO = _mapper.Map<PostPokemonDTO>(model);
            var newPokemons = _mapper.Map<Pokemons>(newPokemonDTO);

            await _uow.Pokemons.AddAsync(newPokemons);
            await _uow.SaveAsync();

            Guid newPokemonID = newPokemons.pokeID;

            //Add PokemonEggGroup
            var checkPokemonEG = false;
            foreach (var pg in model.PokemonEggGroup)
            {
                checkPokemonEG = await _eggGroupService.AddPokemonEggGroup(pg.egID, newPokemonID);
            }
            if (!checkPokemonEG)
            {
                await _uow.Pokemons.RemoveAsync(newPokemons);
                return null;
            }
            //Add PokemonGameVersion
            var checkPokemonGV = false;
            foreach (var gv in model.PokemonGameVersion)
            {
                checkPokemonGV = await _gameVersionService.AddPokemonGameVersion(gv, newPokemonID);
            }
            if (!checkPokemonGV)
            {
                await _uow.Pokemons.RemoveAsync(newPokemons);
                return null;
            }
            //Add PokemonTypes
            var checkPokemonTypes = false;
            foreach (var t in model.PokemonType)
            {
                checkPokemonTypes = await _typeService.AddPokemonType(t.typesID, newPokemonID);
            }
            if (!checkPokemonTypes)
            {
                await _uow.Pokemons.RemoveAsync(newPokemons);
                return null;
            }
            //Add PokemonStats
            var checkPokemonST = false;
            foreach (var pst in model.PokemonStats)
            {
                checkPokemonST = await _statsService.AddPokemonStats(pst, newPokemonID);
            }
            if (!checkPokemonST)
            {
                await _uow.Pokemons.RemoveAsync(newPokemons);
                return null;
            }
            //Add PokemonAbilities
            var checkPokemonA = false;
            foreach (var a in model.PokemonAbilities)
            {
                checkPokemonA = await _abilitiesService.AddPokemonAbilities(a, newPokemonID);
            }
            if (!checkPokemonST)
            {
                await _uow.Pokemons.RemoveAsync(newPokemons);
                return null;
            }
            //Add ImageLink
            var checkImage = false;
            foreach (var a in model.ImageLink)
            {
                checkImage = await _imageService.AddImageLink(a, newPokemonID);
            }
            if (!checkImage)
            {
                await _uow.Pokemons.RemoveAsync(newPokemons);
                return null;
            }
            //Add EffortValues
            var checkEV = false;
            foreach (var ev in model.EffortValues)
            {
                checkEV = await _evService.AddEffortValues(ev, newPokemonID);
            }
            if (!checkEV)
            {
                await _uow.Pokemons.RemoveAsync(newPokemons);
                return null;
            }

            return await _pokemonService.GetPokemonsById(newPokemonID);
        }

        public async Task<bool> PutPokemons(Guid pokeId, PutFullPokemonsDTO model)
        {
            var pokemon = await _uow.Pokemons.GetByIdAsync(pokeId);
            if (pokemon == null)
                return false;

            await _imageService.UpdateImageLink(pokeId, model.ImageLink);
            await _evService.UpdateEffortValues(pokeId, model.EffortValues);
            await _statsService.UpdatePokemonStats(pokeId, model.PokemonStats);
            await _abilitiesService.UpdatePokemonAbilities(pokeId, model.PokemonAbilities);
            await _eggGroupService.UpdatePokemonEggGroup(pokeId, model.PokemonEggGroup);
            await _typeService.UpdatePokemonType(pokeId, model.PokemonType);
            await _moveService.UpdatePokemonMove(pokeId, model.PokemonMove);

            return await _pokemonService.PutPokemons(
                pokeId,
                _mapper.Map<PutPokemonDTO>(model)
            );
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
