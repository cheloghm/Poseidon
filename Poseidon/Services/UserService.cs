using Poseidon.DTOs;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Interfaces.IServices;
using Poseidon.Interfaces.IUtility;
using Poseidon.Models;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Poseidon.Services
{
    public class UserService : Service<User>, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtUtility _jwtUtility;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IJwtUtility jwtUtility, IPasswordHasher<User> passwordHasher, IMapper mapper)
            : base(userRepository)
        {
            _userRepository = userRepository;
            _jwtUtility = jwtUtility;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<User> CreateUserAsync(CreateUserDTO createUserDTO)
        {
             createUserDTO.Id = null;
            // Validate role at the service level
            if (createUserDTO.Role != "Admin" && createUserDTO.Role != "User")
            {
                throw new Exception("Role must be either 'Admin' or 'User'.");
            }

            // Check for unique email and username
            var existingEmail = await _userRepository.GetByEmailAsync(createUserDTO.Email);
            if (existingEmail != null)
            {
                throw new Exception("Email already exists.");
            }

            var existingUsername = await _userRepository.GetByUsernameAsync(createUserDTO.Username);
            if (existingUsername != null)
            {
                throw new Exception("Username already exists.");
            }

            // AutoMapper handles mapping from CreateUserDTO to User
            var user = _mapper.Map<User>(createUserDTO);

            // Hash the password before saving
            user.Password = _passwordHasher.HashPassword(user, createUserDTO.Password);

            await _userRepository.CreateAsync(user);

            return user;
        }

        public async Task UpdateUserAsync(string id, UserDTO userDTO)
        {
            // AutoMapper handles mapping from UserDTO to User
            var user = _mapper.Map<User>(userDTO);

            await _userRepository.UpdateAsync(id, user);
        }

        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {
            var user = await _userRepository.GetByEmailAsync(loginDTO.Email);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.Password, loginDTO.Password) != PasswordVerificationResult.Success)
            {
                return null;
            }

            return _jwtUtility.GenerateJwtToken(user.Id, user.Role);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }
    }
}
