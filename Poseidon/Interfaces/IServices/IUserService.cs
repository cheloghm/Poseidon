using Poseidon.DTOs;
using Poseidon.Models;
using System.Threading.Tasks;

namespace Poseidon.Interfaces.IServices
{
    public interface IUserService : IService<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateUserAsync(CreateUserDTO createUserDTO);
        Task UpdateUserAsync(string id, UserDTO userDTO);
        Task<string> LoginAsync(LoginDTO loginDTO);
    }
}
