using RPD_API.Repo.IRepo;

namespace RPD_API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<IBaseRepository>()
                .AddClasses(classes => classes.AssignableTo<IBaseRepository>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }
    }
}
