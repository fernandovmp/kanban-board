using KanbanBoard.WebApi.Configurations;
using KanbanBoard.WebApi.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace KanbanBoard.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddPostgresDatabase(connectionString: Configuration.GetConnectionString("PostgresConnection"));
            services.AddApplicationCorsPolicy(configuration: Configuration);
            services.AddPasswordHasher(configuration: Configuration);
            services.AddDateTimeProvider();
            services.AddRepositories();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });

            app.UseRouting();

            ApplicationCorsOptions corsOptions = app.ApplicationServices
                .GetService<IOptions<ApplicationCorsOptions>>().Value;
            app.UseCors(corsOptions.PolicyName);

            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/openapi";
                settings.DocumentPath = "/specification/kanban-board-v1.yaml";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
