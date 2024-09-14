using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Poseidon.Data;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Interfaces.IServices;
using Poseidon.Repositories;
using Poseidon.Services;

namespace Poseidon.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfig>(configuration.GetSection("MongoDbConfig"));

            services.AddSingleton<IMongoClient>(sp =>
            {
                var mongoDbConfig = sp.GetRequiredService<IOptions<MongoDbConfig>>().Value;
                return new MongoClient(mongoDbConfig.ConnectionString);
            });

            services.AddSingleton<PoseidonContext>();

            return services;
        }

        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPassengerService, PassengerService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPassengerRepository, PassengerRepository>();

            return services;
        }
    }
}
