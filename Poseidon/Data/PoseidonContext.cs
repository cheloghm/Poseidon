using MongoDB.Driver;
using Poseidon.Config;
using Poseidon.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System;
using Poseidon.Interfaces;

namespace Poseidon.Data
{
    public class PoseidonContext : IPoseidonContext
    {
        private readonly IMongoDatabase _database;
        private readonly ILogger<PoseidonContext> _logger;

        public PoseidonContext(IOptions<DatabaseConfig> config, ILogger<PoseidonContext> logger)
        {
            _logger = logger;

            // Determine the environment
            var isRunningInK8s = Environment.GetEnvironmentVariable("IS_K8S") == "true";
            var isRunningInDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

            string connectionString;

            if (isRunningInK8s)
            {
                connectionString = config.Value.ConnectionString;
            }
            else if (isRunningInDocker)
            {
                connectionString = config.Value.ConnectionStringDocker;
            }
            else
            {
                connectionString = config.Value.ConnectionString;
            }

            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("MongoDB connection string is null or empty.");
                throw new ArgumentNullException(nameof(connectionString), "MongoDB connection string cannot be null");
            }

            _logger.LogInformation("MongoDB Connection String: {ConnectionString}", connectionString);
            _logger.LogInformation("MongoDB Database Name: {DatabaseName}", config.Value.DatabaseName);

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(config.Value.DatabaseName);
        }

        public IMongoDatabase Database => _database;

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Passenger> Passengers => _database.GetCollection<Passenger>("Passengers");
        public IMongoCollection<Token> Tokens => _database.GetCollection<Token>("Tokens");
    }
}
