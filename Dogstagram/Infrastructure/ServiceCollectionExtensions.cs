namespace Dogstagram.Server.Infrastructure
{
    using Dogstagram.Server.Features.Dogs;
    using Dogstagram.Server.Features.Identity;
    using Dogstagram.Server.Data;
    using Dogstagram.Server.Data.Models;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using Microsoft.OpenApi.Models;
    using Dogstagram.Server.Infrastructure.Services;
    using Dogstagram.Server.Features.Profiles;
    using Dogstagram.Server.Features.Follows;
    using Dogstagram.Server.Features.Search;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IIdentityService, IdentityService>()
                .AddTransient<IDogService, DogService>()
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddTransient<IProfileService, ProfileService>()
                .AddTransient<IFollowService, FollowService>()
                .AddTransient<ISearchService, SearchService>();
        }

        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<DogstagramDbContext>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettingsConfiguration = configuration.GetSection("ApplicationSettings");
            services.Configure<ApplicationSettings>(applicationSettingsConfiguration);

            var appSettings = applicationSettingsConfiguration.Get<ApplicationSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Title = "My Dogstagram API",
                        Version = "v1"
                    });
            });

            return services;
        }
    }
}
