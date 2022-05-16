namespace Dogstagram.Server.Infrastructure
{
    using Dogstagram.Server.Data;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My Dogstagram API");
                    options.RoutePrefix = string.Empty;
                });
        }

        public static void ApplyMigrations(this IApplicationBuilder applicationBuilder)
        {
            using var services = applicationBuilder.ApplicationServices.CreateScope();

            var dbContext = services.ServiceProvider.GetService<DogstagramDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
