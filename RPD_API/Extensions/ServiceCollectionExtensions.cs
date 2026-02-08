using Api.Middlewares;
using RPD_API.Repo.IRepo;
using RPD_API.Service.IService;
using Serilog;

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

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.Scan(scan => scan
                .FromAssemblyOf<IBaseService>()
                .AddClasses(classes => classes.AssignableTo<IBaseService>())
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            return services;
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(
        this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }

        public static void ConfigureSeriLog(this IHostBuilder host) {
            host.UseSerilog((ctx, lc) =>
            {
                lc.WriteTo.Console();
            });
        }
    }
}
