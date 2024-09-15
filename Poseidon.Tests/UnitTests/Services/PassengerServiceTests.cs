using Moq;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Models;
using Poseidon.Services;
using System.Collections.Generic;
using System.Linq; // Ensure this is imported to use First(), FirstOrDefault(), or ElementAt()
using System.Threading.Tasks;
using Xunit;

namespace Poseidon.Tests.UnitTests.Services
{
    public class PassengerServiceTests
    {
        private readonly Mock<IPassengerRepository> _mockPassengerRepository;
        private readonly PassengerService _passengerService;

        public PassengerServiceTests()
        {
            _mockPassengerRepository = new Mock<IPassengerRepository>();
            _passengerService = new PassengerService(_mockPassengerRepository.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllPassengers()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe" },
                new Passenger { Id = "2", Name = "Jane Doe" }
            };
            _mockPassengerRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(passengers);

            // Act
            var result = await _passengerService.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetSurvivors_ShouldReturnOnlySurvivors()
        {
            // Arrange
            var survivors = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe", Survived = 1 }
            };
            _mockPassengerRepository.Setup(repo => repo.GetSurvivorsAsync()).ReturnsAsync(survivors);

            // Act
            var result = await _passengerService.GetSurvivorsAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Survived);  // Fixed: Using First() instead of indexing
        }

        [Fact]
        public async Task GetByClass_ShouldReturnPassengersInClass()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe", Pclass = 1 }
            };
            _mockPassengerRepository.Setup(repo => repo.GetByClassAsync(1)).ReturnsAsync(passengers);

            // Act
            var result = await _passengerService.GetByClassAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Pclass);  // Fixed: Using First() instead of indexing
        }

        [Fact]
        public async Task GetByGender_ShouldReturnPassengersWithSpecifiedGender()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe", Sex = "male" }
            };
            _mockPassengerRepository.Setup(repo => repo.GetByGenderAsync("male")).ReturnsAsync(passengers);

            // Act
            var result = await _passengerService.GetByGenderAsync("male");

            // Assert
            Assert.Single(result);
            Assert.Equal("male", result.First().Sex);  // Fixed: Using First() instead of indexing
        }

        [Fact]
        public async Task GetSurvivalRate_ShouldReturnCorrectSurvivalRate()
        {
            // Arrange
            _mockPassengerRepository.Setup(repo => repo.GetSurvivalRateAsync()).ReturnsAsync(50.0);

            // Act
            var result = await _passengerService.GetSurvivalRateAsync();

            // Assert
            Assert.Equal(50.0, result);
        }

        [Fact]
        public async Task Create_ShouldCallCreateInRepository()
        {
            // Arrange
            var passenger = new Passenger { Id = "1", Name = "John Doe" };

            // Act
            await _passengerService.CreateAsync(passenger);

            // Assert
            _mockPassengerRepository.Verify(repo => repo.CreateAsync(passenger), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldCallUpdateInRepository()
        {
            // Arrange
            var passenger = new Passenger { Id = "1", Name = "John Doe" };

            // Act
            await _passengerService.UpdateAsync("1", passenger);

            // Assert
            _mockPassengerRepository.Verify(repo => repo.UpdateAsync("1", passenger), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldCallDeleteInRepository()
        {
            // Act
            await _passengerService.DeleteAsync("1");

            // Assert
            _mockPassengerRepository.Verify(repo => repo.DeleteAsync("1"), Times.Once);
        }
    }
}
