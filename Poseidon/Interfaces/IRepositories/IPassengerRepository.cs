using Poseidon.Models;

namespace Poseidon.Interfaces.IRepositories
{
    public interface IPassengerRepository : IRepository<Passenger>
    {
        // Additional passenger-specific methods if needed
        Task<IEnumerable<Passenger>> GetByClassAsync(int classNumber);
        Task<IEnumerable<Passenger>> GetByGenderAsync(string gender);
        Task<IEnumerable<Passenger>> GetByAgeRangeAsync(double minAge, double maxAge);
        Task<IEnumerable<Passenger>> GetByFareRangeAsync(double minFare, double maxFare);
        Task<IEnumerable<Passenger>> GetSurvivorsAsync();
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
