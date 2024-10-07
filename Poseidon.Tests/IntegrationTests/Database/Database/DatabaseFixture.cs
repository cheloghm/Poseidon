using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Poseidon.Config;
using Poseidon.Data;
using System;
using Xunit;
using Microsoft.Extensions.Logging;
using DotNetEnv;

public class DatabaseFixture : IDisposable
{
    public PoseidonContext Context { get; private set; }

    public DatabaseFixture()
    {
        // Load environment variables from the .env file
        Env.Load();

        // Set up the database configuration
        var config = new DatabaseConfig
        {
            ConnectionString = $"mongodb://{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_USERNAME")}:" +
                               $"{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_PASSWORD")}@localhost:27017",
            DatabaseName = Environment.GetEnvironmentVariable("MONGO_DB_NAME")
        };

        // Use the config to initialize the PoseidonContext
        var options = Options.Create(config);

        // Create a logger instance
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole(); // You can configure logging providers as needed
        });
        ILogger<PoseidonContext> logger = loggerFactory.CreateLogger<PoseidonContext>();

        Context = new PoseidonContext(options, logger);

        // Initialize the database with test data if needed
    }

    public void Dispose()
    {
        // Clean up test database collections
        Context.Passengers.Database.DropCollection("Passengers");
        Context.Users.Database.DropCollection("Users");
        Context.Tokens.Database.DropCollection("Tokens");
    }
}
