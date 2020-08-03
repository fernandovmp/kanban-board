using KanbanBoard.WebApi.Configurations;
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
    }
}
