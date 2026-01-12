using RPD_API.Models;
using RPD_API.Repo.IRepo;
using RPD_API.Repo;
using AutoMapper;

namespace RPD_API.UnitOfWork
{
    public class UnitOfWorkRepo : IUnitOfWorkRepo
    {
        private readonly rpdDbContext _context;
        private readonly IMapper _mapper;

        public UnitOfWorkRepo(rpdDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        private IGrowthRateRepo? _growthRates;
        private ITypesRepo? _types;
        private IStatTypeRepo? _statTypes;
        private IAbilitiesRepo? _abilities;
        private IEggGroupRepo? _eggGroups;
        private IEffortValuesRepo? _effortValues;
        private IGameVersionRepo? _gameVersions;
        private IImageLinkRepo? _imageLinks;
        private IMoveRepo? _moves;
        private IPokemonsRepo? _pokemons;
        private IPokemonAbilitiesRepo? _pokemonAbilities;
        private IPokemonTypeRepo? _pokemonTypes;
        private IPokemonStatsRepo? _pokemonStats;
        private IPokemonEggGroupRepo? _pokemonEggGroups;
        private IPokemonGameVersionRepo? _pokemonGameVersions;
        private IPokemonMoveRepo? _pokemonMoves;
        private IEvolutionChartRepo? _evolutionCharts;

        public IGrowthRateRepo GrowthRates =>
            _growthRates ??= new GrowthRateRepo(_context, _mapper);

        public ITypesRepo Types =>
            _types ??= new TypesRepo(_context, _mapper);

        public IStatTypeRepo StatTypes =>
            _statTypes ??= new StatTypeRepo(_context, _mapper);

        public IAbilitiesRepo Abilities =>
            _abilities ??= new AbilitiesRepo(_context);

        public IEggGroupRepo EggGroups =>
            _eggGroups ??= new EggGroupRepo(_context, _mapper);

        public IEffortValuesRepo EffortValues =>
            _effortValues ??= new EffortValuesRepo(_context);

        public IGameVersionRepo GameVersions =>
            _gameVersions ??= new GameVersionRepo(_context, _mapper);

        public IImageLinkRepo ImageLinks =>
            _imageLinks ??= new ImageLinkRepo(_context, _mapper);

        public IMoveRepo Moves =>
            _moves ??= new MoveRepo(_context, _mapper);

        public IPokemonsRepo Pokemons =>
            _pokemons ??= new PokemonsRepo(_context, _mapper);

        public IPokemonAbilitiesRepo PokemonAbilities =>
            _pokemonAbilities ??= new PokemonAbilitiesRepo(_context, _mapper);

        public IPokemonTypeRepo PokemonTypes =>
            _pokemonTypes ??= new PokemonTypeRepo(_context, _mapper);

        public IPokemonStatsRepo PokemonStats =>
            _pokemonStats ??= new PokemonStatsRepo(_context, _mapper);

        public IPokemonEggGroupRepo PokemonEggGroups =>
            _pokemonEggGroups ??= new PokemonEggGroupRepo(_context, _mapper);

        public IPokemonGameVersionRepo PokemonGameVersions =>
            _pokemonGameVersions ??= new PokemonGameVersionRepo(_context, _mapper);

        public IPokemonMoveRepo PokemonMoves =>
            _pokemonMoves ??= new PokemonMoveRepo(_context, _mapper);

        public IEvolutionChartRepo EvolutionCharts =>
            _evolutionCharts ??= new EvolutionChartRepo(_context, _mapper);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
