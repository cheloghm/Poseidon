using AutoMapper;
using Moq;
using Poseidon.DTOs;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Interfaces.IUtility;
using Poseidon.Models;
using Poseidon.Services;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace Poseidon.Tests.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IJwtUtility> _mockJwtUtility;
        private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;
        private readonly Mock<IMapper> _mockMapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockJwtUtility = new Mock<IJwtUtility>();
            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            _mockMapper = new Mock<IMapper>();
            _userService = new UserService(_mockUserRepository.Object, _mockJwtUtility.Object, _mockPasswordHasher.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CreateUser_ShouldCreateNewUser_WhenEmailAndUsernameAreUnique()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO { Email = "test@example.com", Username = "test_user", Password = "password123" };
            var user = new User { Email = "test@example.com", Username = "test_user", Password = "hashedpassword" };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com")).ReturnsAsync((User)null);
            _mockUserRepository.Setup(repo => repo.GetByUsernameAsync("test_user")).ReturnsAsync((User)null);
            _mockPasswordHasher.Setup(hasher => hasher.HashPassword(It.IsAny<User>(), "password123")).Returns("hashedpassword");
            _mockMapper.Setup(mapper => mapper.Map<User>(createUserDTO)).Returns(user);

            // Act
            var result = await _userService.CreateUserAsync(createUserDTO);

            // Assert
            Assert.Equal("test_user", result.Username);
            _mockUserRepository.Verify(repo => repo.CreateAsync(user), Times.Once);
        }

        [Fact]
        public async Task CreateUser_ShouldThrowException_WhenEmailAlreadyExists()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO { Email = "test@example.com", Username = "test_user" };
            var existingUser = new User { Email = "test@example.com" };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com")).ReturnsAsync(existingUser);

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => _userService.CreateUserAsync(createUserDTO));
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreValid()
        {
            // Arrange
            var loginDTO = new LoginDTO { Email = "test@example.com", Password = "password123" };
            var user = new User { Email = "test@example.com", Password = "hashedpassword" };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com")).ReturnsAsync(user);
            _mockPasswordHasher.Setup(hasher => hasher.VerifyHashedPassword(user, "hashedpassword", "password123")).Returns(PasswordVerificationResult.Success);
            _mockJwtUtility.Setup(jwt => jwt.GenerateJwtToken(user.Id, user.Role)).Returns("jwt_token");

            // Act
            var result = await _userService.LoginAsync(loginDTO);

            // Assert
            Assert.Equal("jwt_token", result);
        }

        [Fact]
        public async Task Login_ShouldReturnNull_WhenCredentialsAreInvalid()
        {
            // Arrange
            var loginDTO = new LoginDTO { Email = "test@example.com", Password = "wrongpassword" };
            var user = new User { Email = "test@example.com", Password = "hashedpassword" };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync("test@example.com")).ReturnsAsync(user);
            _mockPasswordHasher.Setup(hasher => hasher.VerifyHashedPassword(user, "hashedpassword", "wrongpassword")).Returns(PasswordVerificationResult.Failed);

            // Act
            var result = await _userService.LoginAsync(loginDTO);

            // Assert
            Assert.Null(result);
        }
    }
}
