using AutoMapper;
using RPD_API.DTO;
using RPD_API.Models;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;
using RPD_API.UnitOfWork;

namespace RPD_API.Service
{
    public class PokemonService : BaseService, IPokemonService
    {
        public PokemonService(IUnitOfWorkRepo uow, IMapper mapper)
        : base(uow, mapper)
        {
        }

        public async Task<List<PokemonsDTO>> GetAllPokemons()
        {
            var pokemons = await _uow.Pokemons.GetAllAsync();
            return _mapper.Map<List<PokemonsDTO>>(pokemons);
        }

        public async Task<PokemonDetailDTO> GetPokemonsById(Guid pokeID)
        {
            var pokemons = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (pokemons == null)
                return null;
            return _mapper.Map<PokemonDetailDTO>(pokemons);
        }

        public async Task<bool> DeletePokemons(Guid pokeID)
        {
            var Pokemon = await _uow.Pokemons.GetByIdAsync(pokeID);
            if (Pokemon == null)
                return false;

            await _uow.Pokemons.RemoveAsync(Pokemon);

            return await _uow.SaveAsync() > 0;
        }

        public async Task<PokemonsDTO?> PostPokemons(PostPokemonDTO model)
        {
            if (await _uow.Pokemons.ExistsByNationalNumberAsync(model.pokeNationalNumber))
                return null;

            var newPokemons = _mapper.Map<Pokemons>(model);
            await _uow.Pokemons.AddAsync(newPokemons);

            return await _uow.SaveAsync() > 0 ? _mapper.Map<PokemonsDTO?>(newPokemons) : null;
        }

        public async Task<bool> PutPokemons(Guid pokeId, PutPokemonDTO model)
        {
            var pokemons = await _uow.Pokemons.GetByIdAsync(pokeId);
            if (pokemons == null)
                return false;

            _mapper.Map(model, pokemons);

            await _uow.Pokemons.UpdateAsync(pokemons);
            return await _uow.SaveAsync() > 0;
        }
    }
}
