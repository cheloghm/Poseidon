using Poseidon.Interfaces.IRepositories;
using Poseidon.Interfaces.IServices;
using Poseidon.Interfaces.IUtility;
using Poseidon.Models;
using Poseidon.Utilities;
using System.Threading.Tasks;

namespace Poseidon.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtility _jwtUtility;

        public UserService(IUserRepository userRepository, IJwtUtility jwtUtility) : base(userRepository)
        {
            _userRepository = userRepository;
            _jwtUtility = jwtUtility;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            // Validate user credentials
            if (!await _userRepository.ValidateUserCredentials(email, password))
            {
                return null;
            }

            // Generate JWT token
            var user = await GetByEmailAsync(email);
            return _jwtUtility.GenerateJwtToken(user.Id);
        }
    }
}
