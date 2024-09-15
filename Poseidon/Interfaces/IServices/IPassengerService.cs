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
        Task<double> GetSurvivalRateAsync();
    }
}
