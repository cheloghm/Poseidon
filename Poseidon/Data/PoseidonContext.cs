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
            var client = new MongoClient(config.Value.ConnectionString);
            _database = client.GetDatabase(config.Value.DatabaseName);
        }

        public IMongoCollection<Passenger> Passengers => _database.GetCollection<Passenger>("Passengers");
        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");

        // Other collections can be added similarly in the future.
    }
}
