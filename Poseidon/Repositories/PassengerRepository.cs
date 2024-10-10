using MongoDB.Bson;
using MongoDB.Driver;
using Poseidon.Data;
using Poseidon.Interfaces;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poseidon.Repositories
{
    public class PassengerRepository : Repository<Passenger>, IPassengerRepository
    {
        public PassengerRepository(IPoseidonContext context) : base(context.Passengers) { }

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

        public async Task<IEnumerable<Passenger>> SearchPassengersAsync(
            string name = null,
            int? pclass = null,
            string sex = null,
            double? minAge = null,
            double? maxAge = null,
            double? minFare = null,
            double? maxFare = null)
        {
            var filterBuilder = Builders<Passenger>.Filter;
            var filters = new List<FilterDefinition<Passenger>>();

            if (!string.IsNullOrEmpty(name))
            {
                filters.Add(filterBuilder.Regex(p => p.Name, new BsonRegularExpression(name, "i")));
            }

            if (pclass.HasValue)
            {
                filters.Add(filterBuilder.Eq(p => p.Pclass, pclass.Value));
            }

            if (!string.IsNullOrEmpty(sex))
            {
                filters.Add(filterBuilder.Eq(p => p.Sex.ToLower(), sex.ToLower()));
            }

            if (minAge.HasValue)
            {
                filters.Add(filterBuilder.Gte(p => p.Age, minAge.Value));
            }

            if (maxAge.HasValue)
            {
                filters.Add(filterBuilder.Lte(p => p.Age, maxAge.Value));
            }

            if (minFare.HasValue)
            {
                filters.Add(filterBuilder.Gte(p => p.Fare, minFare.Value));
            }

            if (maxFare.HasValue)
            {
                filters.Add(filterBuilder.Lte(p => p.Fare, maxFare.Value));
            }

            var filter = filters.Count > 0 ? filterBuilder.And(filters) : filterBuilder.Empty;

            return await _collection.Find(filter).ToListAsync();
        }

        // New Statistical Methods
        public async Task<int> GetTotalPassengersAsync()
        {
            return (int)await _collection.CountDocumentsAsync(FilterDefinition<Passenger>.Empty);
        }

        public async Task<int> GetNumberOfMenAsync()
        {
            var filter = Builders<Passenger>.Filter.Regex(p => p.Sex, new BsonRegularExpression("^male$", "i"));
            return (int)await _collection.CountDocumentsAsync(filter);
        }

        public async Task<int> GetNumberOfWomenAsync()
        {
            var filter = Builders<Passenger>.Filter.Regex(p => p.Sex, new BsonRegularExpression("^female$", "i"));
            return (int)await _collection.CountDocumentsAsync(filter);
        }

        public async Task<int> GetNumberOfBoysAsync()
        {
            // Assuming 'Age' less than 18 and 'Sex' is male
            var filter = Builders<Passenger>.Filter.And(
                Builders<Passenger>.Filter.Regex(p => p.Sex, new BsonRegularExpression("^male$", "i")),
                Builders<Passenger>.Filter.Lt(p => p.Age, 18)
            );
            return (int)await _collection.CountDocumentsAsync(filter);
        }

        public async Task<int> GetNumberOfGirlsAsync()
        {
            // Assuming 'Age' less than 18 and 'Sex' is female
            var filter = Builders<Passenger>.Filter.And(
                Builders<Passenger>.Filter.Regex(p => p.Sex, new BsonRegularExpression("^female$", "i")),
                Builders<Passenger>.Filter.Lt(p => p.Age, 18)
            );
            return (int)await _collection.CountDocumentsAsync(filter);
        }

        public async Task<int> GetNumberOfAdultsAsync()
        {
            // Assuming 'Age' 18 and above
            var filter = Builders<Passenger>.Filter.Gte(p => p.Age, 18);
            return (int)await _collection.CountDocumentsAsync(filter);
        }

        public async Task<int> GetNumberOfChildrenAsync()
        {
            // Assuming 'Age' below 18
            var filter = Builders<Passenger>.Filter.Lt(p => p.Age, 18);
            return (int)await _collection.CountDocumentsAsync(filter);
        }

        public async Task<double> GetSurvivalRateByAgeRangeAsync(double minAge, double maxAge)
        {
            var filter = Builders<Passenger>.Filter.And(
                Builders<Passenger>.Filter.Gte(p => p.Age, minAge),
                Builders<Passenger>.Filter.Lte(p => p.Age, maxAge)
            );

            var total = await _collection.CountDocumentsAsync(filter);
            if (total == 0) return 0;

            var survivors = await _collection.CountDocumentsAsync(
                Builders<Passenger>.Filter.And(
                    filter,
                    Builders<Passenger>.Filter.Eq(p => p.Survived, 1)
                )
            );

            return (double)survivors / total * 100;
        }

        public async Task<double> GetSurvivalRateByGenderAsync(string sex)
        {
            var filter = Builders<Passenger>.Filter.Regex(p => p.Sex, new BsonRegularExpression($"^{sex}$", "i"));
            var total = await _collection.CountDocumentsAsync(filter);
            if (total == 0) return 0;

            var survivors = await _collection.CountDocumentsAsync(
                Builders<Passenger>.Filter.And(
                    filter,
                    Builders<Passenger>.Filter.Eq(p => p.Survived, 1)
                )
            );

            return (double)survivors / total * 100;
        }

        public async Task<double> GetSurvivalRateByClassAsync(int classNumber)
        {
            var filter = Builders<Passenger>.Filter.Eq(p => p.Pclass, classNumber);
            var total = await _collection.CountDocumentsAsync(filter);
            if (total == 0) return 0;

            var survivors = await _collection.CountDocumentsAsync(
                Builders<Passenger>.Filter.And(
                    filter,
                    Builders<Passenger>.Filter.Eq(p => p.Survived, 1)
                )
            );

            return (double)survivors / total * 100;
        }
    }
}
