using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RPD_API.Models;
using RPD_API.Repo;
using RPD_API.Repo.IRepo;

namespace RPD_API.UnitOfWork
{
    public class UnitOfWorkRepo : IUnitOfWorkRepo
    {
        private readonly rpdDbContext _context;

        public UnitOfWorkRepo(rpdDbContext context)
        {
            _context = context;
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
        private ITrainerRepo? _trainers;
        private IRefreshTokenRepo? _refreshToken;

        public IGrowthRateRepo GrowthRates =>
            _growthRates ??= new GrowthRateRepo(_context);

        public ITypesRepo Types =>
            _types ??= new TypesRepo(_context);

        public IStatTypeRepo StatTypes =>
            _statTypes ??= new StatTypeRepo(_context);

        public IAbilitiesRepo Abilities =>
            _abilities ??= new AbilitiesRepo(_context);

        public IEggGroupRepo EggGroups =>
            _eggGroups ??= new EggGroupRepo(_context);

        public IEffortValuesRepo EffortValues =>
            _effortValues ??= new EffortValuesRepo(_context);

        public IGameVersionRepo GameVersions =>
            _gameVersions ??= new GameVersionRepo(_context);

        public IImageLinkRepo ImageLinks =>
            _imageLinks ??= new ImageLinkRepo(_context);

        public IMoveRepo Moves =>
            _moves ??= new MoveRepo(_context);

        public IPokemonsRepo Pokemons =>
            _pokemons ??= new PokemonsRepo(_context);

        public IPokemonAbilitiesRepo PokemonAbilities =>
            _pokemonAbilities ??= new PokemonAbilitiesRepo(_context);

        public IPokemonTypeRepo PokemonTypes =>
            _pokemonTypes ??= new PokemonTypeRepo(_context);

        public IPokemonStatsRepo PokemonStats =>
            _pokemonStats ??= new PokemonStatsRepo(_context);

        public IPokemonEggGroupRepo PokemonEggGroups =>
            _pokemonEggGroups ??= new PokemonEggGroupRepo(_context);

        public IPokemonGameVersionRepo PokemonGameVersions =>
            _pokemonGameVersions ??= new PokemonGameVersionRepo(_context);

        public IPokemonMoveRepo PokemonMoves =>
            _pokemonMoves ??= new PokemonMoveRepo(_context);

        public IEvolutionChartRepo EvolutionCharts =>
            _evolutionCharts ??= new EvolutionChartRepo(_context);

        public ITrainerRepo Trainers => _trainers ??= new TrainerRepo(_context);

        public IRefreshTokenRepo RefreshTokens => _refreshToken ??= new RefreshTokenRepo(_context);

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
