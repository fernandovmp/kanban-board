using System;
using System.Text;
using KanbanBoard.WebApi.Configurations;
using KanbanBoard.WebApi.Repositories;
using KanbanBoard.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

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
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IBoardRepository, BoardRepository>()
                .AddSingleton<IKanbanTaskRepository, KanbanTaskRepository>()
                .AddSingleton<IKanbanListRepository, KanbanListRepository>()
                .AddSingleton<IBoardMemberRepository, BoardMemberRepository>()
                .AddSingleton<IAssignmentRepository, AssignmentRepository>();

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

        public static IServiceCollection SetupJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            var tokenOptions = new JwtTokenOptions();
            configuration.Bind("JwtToken", tokenOptions);
            byte[] key = Encoding.ASCII.GetBytes(tokenOptions.Key);

            services
                .Configure<JwtTokenOptions>(configuration.GetSection("JwtToken"))
                .AddScoped<ITokenService, TokenService>()
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidAudience = tokenOptions.Audience,
                        ValidIssuer = tokenOptions.Issuer,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            return services;
        }

        public static IServiceCollection SetupApiVersioning(this IServiceCollection services) =>
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(majorVersion: 1, minorVersion: 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            })
            .AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
            });
    }
}
