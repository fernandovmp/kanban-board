using KanbanBoard.WebApi.Configurations;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KanbanBoard.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationCorsPolicy(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var corsOptions = new ApplicationCorsOptions();
            configuration.Bind("CorsOptions", corsOptions);
            return services
                .Configure<ApplicationCorsOptions>(configuration.GetSection("CorsOptions"))
                .AddCors(options =>
                {
                    options.AddPolicy(corsOptions.PolicyName, configurePolicy =>
                    {
                        configurePolicy
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .WithOrigins(corsOptions.AllowedOrigin);
                    });
                });
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services
                .AddSingleton<IUserRepository, UserRepository>();

        public static IServiceCollection AddPostgresDatabase(this IServiceCollection services, string connectionString) =>
            services
                .AddSingleton<IDbConnectionFactory, PostgresDbConnectionFactory>(
                    sp => new PostgresDbConnectionFactory(connectionString));

        public static IServiceCollection AddPasswordHasher(this IServiceCollection services, IConfiguration configuration) =>
            services
                .Configure<HasherOptions>(configuration.GetSection("PasswordHasherOptions"))
                .AddScoped<IPasswordHasherService, PasswordHasherService>();

        public static IServiceCollection AddDateTimeProvider(this IServiceCollection services) =>
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

    }
}
