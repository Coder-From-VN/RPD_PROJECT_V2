using RPD_API.Repo.IRepo;

namespace RPD_API.Service
{
    public class PokemonService
    {
        private readonly IPokemonsRepo _pokeRepo;

        public PokemonService(IPokemonsRepo pokeRepo)
        {
            _pokeRepo = pokeRepo;
        }
    }
}
