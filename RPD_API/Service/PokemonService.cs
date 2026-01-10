using AutoMapper;
using RPD_API.DTO;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonService : IPokemonService
    {
        private readonly IUnitOfWorkRepo _uow;
        private readonly IMapper _mapper;

        public PokemonService(IUnitOfWorkRepo uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PokemonDetailDTO> PostFullPokemons(PostFullPokemonsDTO model)
        {
            var newPokemon = _uow.Pokemons.AddPokemons(_mapper.Map<PostPokemonDTO>(model));
            Guid newPokemonID = newPokemon.Result.pokeID;
            if (newPokemon == null)
                return null;
            //Add PokemonEggGroup
            var checkPokemonEG = false;
            foreach (var pg in model.PokemonEggGroup)
            {
                checkPokemonEG = _uow.PokemonEggGroups.AddPokemonEggGroup(pg.egID, newPokemonID).Result;
            }
            if (!checkPokemonEG)
            {
                await _uow.Pokemons.DeletePokemons(newPokemon.Result.pokeID);
                return null;
            }
            //Add PokemonGameVersion
            var checkPokemonGV = false;
            foreach (var gv in model.PokemonGameVersion)
            {
                checkPokemonGV = _uow.PokemonGameVersions.AddPokemonGameVersion(gv, newPokemonID).Result;
            }
            if (!checkPokemonGV)
            {
                await _uow.Pokemons.DeletePokemons(newPokemonID);
                return null;
            }
            //Add PokemonMove this should not be here right?
            var checkPokemonM = false;
            foreach (var m in model.PokemonMove)
            {
                checkPokemonM = _uow.PokemonMoves.AddPokemonMove(m, newPokemonID).Result;
            }
            if (!checkPokemonM)
            {
                await _uow.Pokemons.DeletePokemons(newPokemonID);
                return null;
            }
            //Add PokemonTypes
            var checkPokemonTypes = false;
            foreach (var t in model.PokemonType)
            {
                checkPokemonTypes = _uow.PokemonTypes.AddPokemonType(t.typesID, newPokemonID).Result;
            }
            if (!checkPokemonTypes)
            {
                await _uow.Pokemons.DeletePokemons(newPokemonID);
                return null;
            }
            //Add PokemonStats
            var checkPokemonST = false;
            foreach (var pst in model.PokemonStats)
            {
                checkPokemonST = _uow.PokemonStats.AddPokemonStats(pst, newPokemonID).Result;
            }
            if (!checkPokemonST)
            {
                await _uow.Pokemons.DeletePokemons(newPokemonID);
                return null;
            }
            //Add PokemonAbilities
            var checkPokemonA = false;
            foreach (var a in model.PokemonAbilities)
            {
                checkPokemonA = _uow.PokemonAbilities.AddPokemonAbilities(a, newPokemonID).Result;
            }
            if (!checkPokemonST)
            {
                await _uow.Pokemons.DeletePokemons(newPokemonID);
                return null;
            }
            //Add ImageLink
            var checkImage = false;
            foreach (var a in model.ImageLink)
            {
                checkImage = _uow.ImageLinks.AddImageLink(a, newPokemonID).Result;
            }
            if (!checkImage)
            {
                await _uow.Pokemons.DeletePokemons(newPokemonID);
                return null;
            }
            //Add EffortValues
            var checkEV = false;
            foreach (var ev in model.EffortValues)
            {
                checkEV = _uow.EffortValues.AddEffortValues(ev, newPokemonID).Result;
            }
            if (!checkEV)
            {
                await _uow.Pokemons.DeletePokemons(newPokemonID);
                return null;
            }
            return await _uow.Pokemons.GetPokemonsById(newPokemonID);

        }

        public async Task<PokemonsDTO> PutFullPokemons(Guid pokeId, PutFullPokemonsDTO model)
        {
            var pokemon = await _uow.Pokemons.FindPokemonsById(pokeId);
            if (pokemon == null)
                return null;
            //Put PutImageLinkDTO
            await _uow.ImageLinks.UpdateImageLink(pokeId, model.ImageLink);
            //Put EV
            await _uow.EffortValues.UpdateEffortValues(pokeId, model.EffortValues);
            //Put PokemonStats
            await _uow.PokemonStats.UpdatePokemonStats(pokeId, model.PokemonStats);
            //Put PokemonAbilities
            await _uow.PokemonAbilities.UpdatePokemonAbilities(pokeId, model.PokemonAbilities);
            //Put PokemonEggGroup
            await _uow.PokemonEggGroups.UpdatePokemonEggGroup(pokeId, model.PokemonEggGroup);
            //Put PokemonType
            await _uow.PokemonTypes.UpdatePokemonType(pokeId, model.PokemonType);
            //Put pokemonMove
            await _uow.PokemonMoves.UpdatePokemonMove(pokeId, model.PokemonMove);
            //Put pokemon
            await _uow.Pokemons.UpdatePokemons(pokeId, _mapper.Map<PutPokemonDTO>(model));

            return _mapper.Map<PokemonsDTO>(await _uow.Pokemons.FindPokemonsById(pokeId));

        }

        public async Task<bool> DeleteFullPokemons(Guid pokeID)
        {
            var pokemon = await _uow.Pokemons.FindPokemonsById(pokeID);
            if (pokemon == null)
                return false;
            var check = false;
            //Delete ImageLink
            foreach (var item in pokemon.ImageLink)
            {
                check = await _uow.ImageLinks.DeleteImageLink(item.pokeID);
            }
            //Delete EV
            foreach (var item in pokemon.EffortValues)
            {
                check = await _uow.EffortValues.DeleteEffortValues(item.pokeID);
            }
            //Delete PokemonStats
            foreach (var item in pokemon.PokemonStats)
            {
                check = await _uow.PokemonStats.DeletePokemonStats(pokeID, item.stID);
            }
            //Delete PokemonAbilities
            foreach (var item in pokemon.PokemonAbilities)
            {
                await _uow.PokemonAbilities.DeletePokemonAbilities(pokeID, item.abID);
            }
            //Delete PokemonEggGroup
            foreach (var item in pokemon.PokemonEggGroup)
            {
                check = await _uow.PokemonEggGroups.DeletePokemonEggGroup(pokeID, item.egID);
            }
            //Delete PokemonType
            foreach (var item in pokemon.PokemonType)
            {
                check = await _uow.PokemonTypes.DeletePokemonType(pokeID, item.typesID);
            }
            //Delete PokemonMove
            foreach (var item in pokemon.PokemonMove)
            {
                check = await _uow.PokemonMoves.DeletePokemonMove(pokeID, item.moveID);
            }
            //Delete Pokemon
            check = await _uow.Pokemons.DeletePokemons(pokeID);

            return check;
        }
    }
}
