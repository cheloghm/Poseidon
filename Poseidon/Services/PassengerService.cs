using Poseidon.Interfaces.IRepositories;
using Poseidon.Interfaces.IServices;
using Poseidon.Models;

namespace Poseidon.Services
{
    public class PassengerService : Service<Passenger>, IPassengerService
    {
        private readonly IPassengerRepository _passengerRepository;

        public PassengerService(IPassengerRepository passengerRepository) : base(passengerRepository)
        {
            _passengerRepository = passengerRepository;
        }
    }
}
