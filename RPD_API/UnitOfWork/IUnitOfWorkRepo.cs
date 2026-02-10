using Microsoft.EntityFrameworkCore.Storage;
using RPD_API.Repo.IRepo;

namespace RPD_API.UnitOfWork
{
    public interface IUnitOfWorkRepo
    {
        IGrowthRateRepo GrowthRates { get; }
        ITypesRepo Types { get; }
        IStatTypeRepo StatTypes { get; }
        IAbilitiesRepo Abilities { get; }
        IEggGroupRepo EggGroups { get; }
        IEffortValuesRepo EffortValues { get; }
        IGameVersionRepo GameVersions { get; }
        IImageLinkRepo ImageLinks { get; }
        IMoveRepo Moves { get; }
        IPokemonsRepo Pokemons { get; }
        IPokemonAbilitiesRepo PokemonAbilities { get; }
        IPokemonTypeRepo PokemonTypes { get; }
        IPokemonStatsRepo PokemonStats { get; }
        IPokemonEggGroupRepo PokemonEggGroups { get; }
        IPokemonGameVersionRepo PokemonGameVersions { get; }
        IPokemonMoveRepo PokemonMoves { get; }
        IEvolutionChartRepo EvolutionCharts { get; }
        ITrainerRepo Trainers { get; }
        IRefreshTokenRepo RefreshTokens { get; }
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> SaveAsync();
    }
}
