using MongoDB.Driver;
using Poseidon.Config;
using Poseidon.Models;
using Microsoft.Extensions.Options;

namespace Poseidon.Data
{
    public class PoseidonContext
    {
        private readonly IMongoDatabase _database;

        public PoseidonContext(IOptions<DatabaseConfig> config)
        {
            if (string.IsNullOrEmpty(config.Value.ConnectionString))
                throw new ArgumentNullException(nameof(config.Value.ConnectionString), "MongoDB connection string cannot be null");

            var client = new MongoClient(config.Value.ConnectionString);
            _database = client.GetDatabase(config.Value.DatabaseName);
        }

        public IMongoDatabase Database => _database;

        // Make the collections virtual to allow overriding in tests
        public virtual IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public virtual IMongoCollection<Passenger> Passengers => _database.GetCollection<Passenger>("Passengers");
        public virtual IMongoCollection<Token> Tokens => _database.GetCollection<Token>("Tokens");
    }

}
