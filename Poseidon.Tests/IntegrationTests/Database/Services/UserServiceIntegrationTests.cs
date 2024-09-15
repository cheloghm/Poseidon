using System.Threading.Tasks;
using Moq;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Interfaces.IUtility;
using Poseidon.Interfaces.IServices;
using Poseidon.Services;
using Poseidon.DTOs;
using Poseidon.Models;
using Xunit;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Poseidon.Tests.IntegrationTests.Services
{
    public class UserServiceIntegrationTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IJwtUtility> _mockJwtUtility;
        private readonly Mock<IPasswordHasher<User>> _mockPasswordHasher;
        private readonly Mock<IMapper> _mockMapper;

        public UserServiceIntegrationTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockJwtUtility = new Mock<IJwtUtility>();
            _mockPasswordHasher = new Mock<IPasswordHasher<User>>();
            _mockMapper = new Mock<IMapper>();

            _userService = new UserService(
                _mockUserRepository.Object,
                _mockJwtUtility.Object,
                _mockPasswordHasher.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCreateNewUser()
        {
            // Arrange
            var createUserDto = new CreateUserDTO { Email = "test@test.com", Username = "testUser", Password = "password123" };
            var user = new User { Id = "1", Email = "test@test.com", Username = "testUser" };

            _mockMapper.Setup(m => m.Map<User>(createUserDto)).Returns(user);
            _mockPasswordHasher.Setup(h => h.HashPassword(user, createUserDto.Password)).Returns("hashedPassword");

            // Act
            var result = await _userService.CreateUserAsync(createUserDto);

            // Assert
            Assert.NotNull(result);
            _mockUserRepository.Verify(repo => repo.CreateAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnTokenOnSuccess()
        {
            // Arrange
            var loginDto = new LoginDTO { Email = "test@test.com", Password = "password123" };
            var user = new User { Id = "1", Email = "test@test.com", Password = "hashedPassword" };

            _mockUserRepository.Setup(repo => repo.GetByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _mockPasswordHasher.Setup(h => h.VerifyHashedPassword(user, user.Password, loginDto.Password))
                .Returns(PasswordVerificationResult.Success);
            _mockJwtUtility.Setup(jwt => jwt.GenerateJwtToken(user.Id, user.Role)).Returns("fake-jwt-token");

            // Act
            var result = await _userService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("fake-jwt-token", result);
            _mockUserRepository.Verify(repo => repo.GetByEmailAsync(loginDto.Email), Times.Once);
        }
    }
}
