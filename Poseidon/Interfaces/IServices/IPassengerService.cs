﻿using Poseidon.DTOs;
using Poseidon.Models;

namespace Poseidon.Interfaces.IServices
{
    public interface IPassengerService : IService<Passenger>
    {
        Task<IEnumerable<Passenger>> GetByClassAsync(int classNumber);
        Task<IEnumerable<Passenger>> GetByGenderAsync(string sex);
        Task<IEnumerable<Passenger>> GetByAgeRangeAsync(double minAge, double maxAge);
        Task<IEnumerable<Passenger>> GetByFareRangeAsync(double minFare, double maxFare);
        Task<IEnumerable<Passenger>> GetSurvivorsAsync();
        Task<Passenger> CreatePassengerAsync(PassengerDTO passengerDTO);
        Task<double> GetSurvivalRateAsync();
        Task<IEnumerable<Passenger>> SearchPassengersAsync(
            string name = null,
            int? pclass = null,
            string sex = null,
            double? minAge = null,
            double? maxAge = null,
            double? minFare = null,
            double? maxFare = null);
    }

}
