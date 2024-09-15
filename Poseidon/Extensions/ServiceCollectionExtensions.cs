using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Poseidon.Config;
using Poseidon.Data;
using Poseidon.Interfaces.IServices;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Repositories;
using Poseidon.Services;
using Poseidon.Mappers;
using Poseidon.Utilities;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Poseidon.Interfaces.IUtility;
using Poseidon.BackgroundTasks;
using Poseidon.Interfaces;

namespace Poseidon.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseConfig>(configuration.GetSection("DatabaseConfig"));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var mongoDbConfig = sp.GetRequiredService<IOptions<DatabaseConfig>>().Value;
                return new MongoClient(mongoDbConfig.ConnectionString);
            });

            services.AddSingleton<PoseidonContext>();

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("Jwt");

            var key = Encoding.UTF8.GetBytes(jwtSection["Key"]);
            var issuer = jwtSection["Issuer"];
            var audience = jwtSection["Audience"];

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            services.AddSingleton<IJwtUtility>(sp => new JwtUtility(key, issuer, audience));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPassengerService, PassengerService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();

            return services;
        }

        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        public static IServiceCollection AddBackgroundTasks(this IServiceCollection services)
        {
            services.AddHostedService<CleanupExpiredTokensTask>();
            // Add other background tasks as necessary
            return services;
        }
    }
}
