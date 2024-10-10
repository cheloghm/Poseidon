using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Poseidon.DTOs;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Interfaces.IServices;
using Poseidon.Models;
using Poseidon.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Poseidon.Services
{
    public class PassengerService : Service<Passenger>, IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;
        private readonly IMapper _mapper;

        public PassengerService(IPassengerRepository passengerRepository, IMapper mapper) : base(passengerRepository)
        {
            _passengerRepository = passengerRepository;
            _mapper = mapper; // Make sure to initialize the _mapper
        }

        public async Task<IEnumerable<Passenger>> GetByClassAsync(int classNumber)
        {
            return await _passengerRepository.GetByClassAsync(classNumber);
        }

        public async Task<IEnumerable<Passenger>> GetByGenderAsync(string sex)
        {
            return await _passengerRepository.GetByGenderAsync(sex);
        }

        public async Task<IEnumerable<Passenger>> GetByAgeRangeAsync(double minAge, double maxAge)
        {
            return await _passengerRepository.GetByAgeRangeAsync(minAge, maxAge);
        }

        public async Task<IEnumerable<Passenger>> GetByFareRangeAsync(double minFare, double maxFare)
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

        public async Task<Passenger> CreatePassengerAsync(PassengerDTO passengerDTO)
        {
            passengerDTO.Id = null; // Ensure the ID is set to null for MongoDB to generate it

            // AutoMapper maps the DTO to the Passenger model
            var passenger = _mapper.Map<Passenger>(passengerDTO);
            await _passengerRepository.CreateAsync(passenger);

            return passenger; // Return the created Passenger
        }

        public async Task UpdateAsync(string id, Passenger passenger)
        {
            await _passengerRepository.UpdateAsync(id, passenger);
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
            return await _passengerRepository.SearchPassengersAsync(
                name, pclass, sex, minAge, maxAge, minFare, maxFare);
        }

        // Statistical Methods
        public async Task<int> GetTotalPassengersAsync()
        {
            return await _passengerRepository.GetTotalPassengersAsync();
        }

        public async Task<int> GetNumberOfSurvivorsAsync()
        {
            return await _passengerRepository.GetNumberOfSurvivorsAsync();
        }

        public async Task<int> GetNumberOfMenAsync()
        {
            return await _passengerRepository.GetNumberOfMenAsync();
        }

        public async Task<int> GetNumberOfWomenAsync()
        {
            return await _passengerRepository.GetNumberOfWomenAsync();
        }

        public async Task<int> GetNumberOfBoysAsync()
        {
            return await _passengerRepository.GetNumberOfBoysAsync();
        }

        public async Task<int> GetNumberOfGirlsAsync()
        {
            return await _passengerRepository.GetNumberOfGirlsAsync();
        }

        public async Task<int> GetNumberOfAdultsAsync()
        {
            return await _passengerRepository.GetNumberOfAdultsAsync();
        }

        public async Task<int> GetNumberOfChildrenAsync()
        {
            return await _passengerRepository.GetNumberOfChildrenAsync();
        }

        public async Task<double> GetSurvivalRateByAgeRangeAsync(double minAge, double maxAge)
        {
            return await _passengerRepository.GetSurvivalRateByAgeRangeAsync(minAge, maxAge);
        }

        public async Task<double> GetSurvivalRateByGenderAsync(string sex)
        {
            return await _passengerRepository.GetSurvivalRateByGenderAsync(sex);
        }

        public async Task<double> GetSurvivalRateByClassAsync(int classNumber)
        {
            return await _passengerRepository.GetSurvivalRateByClassAsync(classNumber);
        }

        public async Task<IEnumerable<Passenger>> GetPassengersByAgeRangeAsync(double minAge, double maxAge)
        {
            return await _passengerRepository.GetPassengersByAgeRangeAsync(minAge, maxAge);
        }

        public async Task<IEnumerable<Passenger>> GetPassengersByFareRangeAsync(double minFare, double maxFare)
        {
            return await _passengerRepository.GetPassengersByFareRangeAsync(minFare, maxFare);
        }

    }
}
