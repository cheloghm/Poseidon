using System.Collections.Generic;
using System.Linq;  // Make sure to include this for LINQ methods like Count()
using System.Threading.Tasks;
using MongoDB.Bson;
using Xunit;
using Poseidon.Services;
using Poseidon.Models;
using Poseidon.Interfaces.IRepositories;
using Moq;
using AutoMapper;  // Import AutoMapper

namespace Poseidon.Tests.UnitTests.Services
{
    public class PassengerServiceTests
    {
        private readonly PassengerService _passengerService;
        private readonly Mock<IPassengerRepository> _mockPassengerRepository;
        private readonly Mock<IMapper> _mockMapper; // Add Mock for IMapper

        public PassengerServiceTests()
        {
            _mockPassengerRepository = new Mock<IPassengerRepository>();
            _mockMapper = new Mock<IMapper>();  // Initialize the IMapper Mock
            _passengerService = new PassengerService(_mockPassengerRepository.Object, _mockMapper.Object); // Pass the IMapper mock into the constructor
        }

        [Fact]
        public async Task GetByClassAsync_ShouldReturnPassengersOfSpecifiedClass()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = ObjectId.GenerateNewId().ToString(), Pclass = 1, Name = "John Doe" },
                new Passenger { Id = ObjectId.GenerateNewId().ToString(), Pclass = 1, Name = "Jane Doe" }
            };
            _mockPassengerRepository.Setup(repo => repo.GetByClassAsync(1)).ReturnsAsync(passengers);

            // Act
            var result = await _passengerService.GetByClassAsync(1);

            // Assert
            Assert.Equal(2, result.Count());  // Correct usage of Count() method
            _mockPassengerRepository.Verify(repo => repo.GetByClassAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetSurvivorsAsync_ShouldReturnSurvivors()
        {
            // Arrange
            var survivors = new List<Passenger>
            {
                new Passenger { Id = ObjectId.GenerateNewId().ToString(), Survived = 1, Name = "John Doe" }
            };
            _mockPassengerRepository.Setup(repo => repo.GetSurvivorsAsync()).ReturnsAsync(survivors);

            // Act
            var result = await _passengerService.GetSurvivorsAsync();

            // Assert
            Assert.Single(result);
            _mockPassengerRepository.Verify(repo => repo.GetSurvivorsAsync(), Times.Once);
        }

        [Fact]
        public async Task GetSurvivalRateAsync_ShouldReturnCorrectRate()
        {
            // Arrange
            var expectedRate = 75.0;
            _mockPassengerRepository.Setup(repo => repo.GetSurvivalRateAsync()).ReturnsAsync(expectedRate);

            // Act
            var result = await _passengerService.GetSurvivalRateAsync();

            // Assert
            Assert.Equal(expectedRate, result);
            _mockPassengerRepository.Verify(repo => repo.GetSurvivalRateAsync(), Times.Once);
        }
    }
}
