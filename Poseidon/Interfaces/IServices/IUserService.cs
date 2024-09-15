using Poseidon.Models;
using System.Threading.Tasks;

namespace Poseidon.Interfaces.IServices
{
    public interface IUserService : IService<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<string> LoginAsync(string email, string password);
    }
}
