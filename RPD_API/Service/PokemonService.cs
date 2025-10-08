using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;

namespace RPD_API.Service
{
    public class PokemonService
    {
        private readonly IPokemonsRepo _pokeRepo;
        private readonly IPokemonEggGroupRepo _egRepo;
        private readonly IPokemonGameVersionRepo _gvRepo;
        private readonly IPokemonMoveRepo _pmRepo;
        private readonly IPokemonTypeRepo _ptRepo;
        private readonly IPokemonStatsRepo _pstRepo;
        private readonly IPokemonAbilitiesRepo _abRepo;
        private readonly IImageLinkRepo _imgRepo;
        private readonly IEffortValuesRepo _evRepo;
        private readonly IMapper _mapper;

        public PokemonService(IPokemonsRepo pokeRepo,
            IPokemonGameVersionRepo gvRepo,
            IPokemonMoveRepo pmRepo,
            IPokemonEggGroupRepo egRepo,
            IPokemonTypeRepo ptRepo,
            IPokemonStatsRepo pstRepo,
            IPokemonAbilitiesRepo abRepo,
            IImageLinkRepo imgRepo,
            IEffortValuesRepo evRepo,
            IMapper mapper)
        {
            _pokeRepo = pokeRepo;
            _egRepo = egRepo;
            _gvRepo = gvRepo;
            _pmRepo = pmRepo;
            _ptRepo = ptRepo;
            _pstRepo = pstRepo;
            _abRepo = abRepo;
            _imgRepo = imgRepo;
            _evRepo = evRepo;
            _mapper = mapper;
        }

        public async Task<PokemonsDTO> PostFullPokemons(PostFullPokemonsDTO model)
        {
            var newPokemon = _pokeRepo.AddPokemons(_mapper.Map<PostPokemonDTO>(model));
            Guid newPokemonID = newPokemon.Result.pokeID;
            if (newPokemon == null)
                return null;
            //Add PokemonEggGroup
            var checkPokemonEG = false;
            foreach (var pg in model.PokemonEggGroup)
            {
                checkPokemonEG = _egRepo.AddPokemonEggGroup(pg.egID, newPokemonID).Result;
            }
            if (!checkPokemonEG)
            {
                await _pokeRepo.DeletePokemons(newPokemon.Result.pokeID);
                return null;
            }
            //Add PokemonGameVersion
            var checkPokemonGV = false;
            foreach (var gv in model.PokemonGameVersion)
            {
                checkPokemonGV = _gvRepo.AddPokemonGameVersion(gv, newPokemonID).Result;
            }
            if (!checkPokemonGV)
            {
                await _pokeRepo.DeletePokemons(newPokemonID);
                return null;
            }
            //Add PokemonMove
            var checkPokemonM = false;
            foreach (var m in model.PokemonMove)
            {
                checkPokemonM = _pmRepo.AddPokemonMove(m, newPokemonID).Result;
            }
            if (!checkPokemonM)
            {
                await _pokeRepo.DeletePokemons(newPokemonID);
                return null;
            }
            //Add PokemonTypes
            var checkPokemonTypes = false;
            foreach (var t in model.PokemonType)
            {
                checkPokemonTypes = _ptRepo.AddPokemonType(t.typesID, newPokemonID).Result;
            }
            if (!checkPokemonTypes)
            {
                await _pokeRepo.DeletePokemons(newPokemonID);
                return null;
            }
            //Add PokemonStats
            var checkPokemonST = false;
            foreach (var pst in model.PokemonStats)
            {
                checkPokemonST = _pstRepo.AddPokemonStats(pst, newPokemonID).Result;
            }
            if (!checkPokemonST)
            {
                await _pokeRepo.DeletePokemons(newPokemonID);
                return null;
            }
            //Add PokemonAbilities
            var checkPokemonA = false;
            foreach (var a in model.PokemonAbilities)
            {
                checkPokemonA = _abRepo.AddPokemonAbilities(a, newPokemonID).Result;
            }
            if (!checkPokemonST)
            {
                await _pokeRepo.DeletePokemons(newPokemonID);
                return null;
            }
            //Add ImageLink
            var checkImage = false;
            foreach (var a in model.ImageLink)
            {
                checkImage = _imgRepo.AddImageLink(a, newPokemonID).Result;
            }
            if (!checkImage)
            {
                await _pokeRepo.DeletePokemons(newPokemonID);
                return null;
            }
            //Add EffortValues
            var checkEV = false;
            foreach (var ev in model.EffortValues)
            {
                checkEV = _evRepo.AddEffortValues(ev, newPokemonID).Result;
            }
            if (!checkEV)
            {
                await _pokeRepo.DeletePokemons(newPokemonID);
                return null;
            }

            return await _pokeRepo.GetPokemonsById(newPokemonID);

        }

        public async Task<PokemonsDTO> PutFullPokemons(Guid pokeId, PutFullPokemonsDTO model)
        {
            var pokemon = await _pokeRepo.FindPokemonsById(pokeId);
            if (pokemon == null)
                return null;
            //Put PutImageLinkDTO
            await _imgRepo.UpdateImageLink(pokeId, model.ImageLink);
            //Put EV
            await _evRepo.UpdateEffortValues(pokeId, model.EffortValues);
            //Put PokemonStats
            await _pstRepo.UpdatePokemonStats(pokeId, model.PokemonStats);

            return _mapper.Map<PokemonsDTO>(await _pokeRepo.FindPokemonsById(pokeId));

        }

        public async Task<bool> DeletePokemons(Guid pokeID)
        {
            var pokemon = await _pokeRepo.FindPokemonsById(pokeID);
            if (pokemon == null)
                return false;
            //Delete ImageLink
            foreach (var item in pokemon.ImageLink)
            {
                await _imgRepo.DeleteImageLink(item.pokeID);
            }
            //Delete EV
            foreach (var item in pokemon.EffortValues)
            {
                await _imgRepo.DeleteImageLink(item.evID);
            }
            //Delete PokemonStats
            foreach (var item in pokemon.PokemonStats)
            {
                await _pstRepo.DeletePokemonStats(pokeID, item.stID);
            }


            return false;
        }
    }
}
