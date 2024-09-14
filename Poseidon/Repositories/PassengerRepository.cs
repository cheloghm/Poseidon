using MongoDB.Driver;
using Poseidon.Data;
using Poseidon.Interfaces.Repositories;
using Poseidon.Models;

namespace Poseidon.Repositories
{
    public class PassengerRepository : Repository<Passenger>, IPassengerRepository
    {
        public PassengerRepository(PoseidonContext context) : base(context.Passengers)
        {
        }
    }
}
