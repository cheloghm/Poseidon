using MongoDB.Driver;
using Poseidon.Models;

namespace Poseidon.Interfaces
{
    public interface IPoseidonContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Passenger> Passengers { get; }
        IMongoCollection<Token> Tokens { get; }
    }
}
