using Poseidon.Interfaces.Repositories;
using Poseidon.Interfaces.Services;
using Poseidon.Models;
using Poseidon.Repositories;
using System.Threading.Tasks;

namespace Poseidon.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }
    }
}
