using MongoDB.Bson;
using MongoDB.Driver;
using Poseidon.Data;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poseidon.Repositories
{
    public class PassengerRepository : Repository<Passenger>, IPassengerRepository
    {
        public PassengerRepository(PoseidonContext context) : base(context.Passengers) { }

        public async Task<IEnumerable<Passenger>> GetByClassAsync(int classNumber)
        {
            return await _collection.Find(p => p.Pclass == classNumber).ToListAsync();
        }

        public async Task<IEnumerable<Passenger>> GetByGenderAsync(string sex)
        {
            return await _collection.Find(p => p.Sex.ToLower() == sex.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Passenger>> GetByAgeRangeAsync(double minAge, double maxAge)
        {
            return await _collection.Find(p => p.Age >= minAge && p.Age <= maxAge).ToListAsync();
        }

        public async Task<IEnumerable<Passenger>> GetByFareRangeAsync(double minFare, double maxFare)
        {
            var filter = Builders<Passenger>.Filter.And(
                Builders<Passenger>.Filter.Gte(p => p.Fare, minFare),
                Builders<Passenger>.Filter.Lte(p => p.Fare, maxFare)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Passenger>> GetSurvivorsAsync()
        {
            return await _collection.Find(p => p.Survived == 1).ToListAsync();
        }

        public async Task<double> GetSurvivalRateAsync()
        {
            var totalPassengers = await _collection.CountDocumentsAsync(Builders<Passenger>.Filter.Empty);
            var survivors = await _collection.CountDocumentsAsync(Builders<Passenger>.Filter.Eq("Survived", 1));

            return (double)survivors / totalPassengers * 100;
        }

        public async Task UpdateAsync(string id, Passenger passenger)
        {
            var objectId = ObjectId.Parse(id); // Parse string ID to ObjectId
            var filter = Builders<Passenger>.Filter.Eq("_id", objectId); // Use "_id" for ObjectId filter
            var result = await _collection.ReplaceOneAsync(filter, passenger);

            if (result.MatchedCount == 0)
            {
                throw new Exception($"No passenger found with ID: {id}");
            }
        }

    }
}
