using Poseidon.Models;
using System.Threading.Tasks;

namespace Poseidon.Interfaces.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByUsernameAsync(string username);
        Task<bool> ValidateUserCredentials(string email, string password);
    }
}
