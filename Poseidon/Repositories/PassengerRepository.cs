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

        public async Task<IEnumerable<Passenger>> GetByAgeRangeAsync(int minAge, int maxAge)
        {
            return await _collection.Find(p => p.Age >= minAge && p.Age <= maxAge).ToListAsync();
        }

        public async Task<IEnumerable<Passenger>> GetByFareRangeAsync(decimal minFare, decimal maxFare)
        {
            return await _collection.Find(p => (decimal)p.Fare >= minFare && (decimal)p.Fare <= maxFare).ToListAsync();
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
    }
}
