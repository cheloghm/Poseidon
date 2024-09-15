using Poseidon.Interfaces.IRepositories;
using Poseidon.Interfaces.IServices;
using Poseidon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Poseidon.Services
{
    public class PassengerService : Service<Passenger>, IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;

        public PassengerService(IPassengerRepository passengerRepository) : base(passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }

        public async Task<IEnumerable<Passenger>> GetByClassAsync(int classNumber)
        {
            return await _passengerRepository.GetByClassAsync(classNumber);
        }

        public async Task<IEnumerable<Passenger>> GetByGenderAsync(string sex)
        {
            return await _passengerRepository.GetByGenderAsync(sex);
        }

        public async Task<IEnumerable<Passenger>> GetByAgeRangeAsync(int minAge, int maxAge)
        {
            return await _passengerRepository.GetByAgeRangeAsync(minAge, maxAge);
        }

        public async Task<IEnumerable<Passenger>> GetByFareRangeAsync(decimal minFare, decimal maxFare)
        {
            return await _passengerRepository.GetByFareRangeAsync(minFare, maxFare);
        }

        public async Task<IEnumerable<Passenger>> GetSurvivorsAsync()
        {
            return await _passengerRepository.GetSurvivorsAsync();
        }

        public async Task<double> GetSurvivalRateAsync()
        {
            return await _passengerRepository.GetSurvivalRateAsync();
        }
    }
}
