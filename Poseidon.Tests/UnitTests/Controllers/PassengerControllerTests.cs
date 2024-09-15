using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Poseidon.Controllers;
using Poseidon.DTOs;
using Poseidon.Interfaces.IServices;
using Poseidon.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Poseidon.Tests.UnitTests.Controllers
{
    public class PassengerControllerTests
    {
        private readonly Mock<IPassengerService> _mockPassengerService;
        private readonly PassengerController _controller;

        public PassengerControllerTests()
        {
            _mockPassengerService = new Mock<IPassengerService>();
            _controller = new PassengerController(_mockPassengerService.Object, null); // AutoMapper is not used in these tests.
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkResult_WithPaginatedPassengers()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe", Pclass = 1 },
                new Passenger { Id = "2", Name = "Jane Doe", Pclass = 2 }
            };
            _mockPassengerService.Setup(service => service.GetAllAsync()).ReturnsAsync(passengers);

            // Act
            var result = await _controller.GetAll(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPassengers = Assert.IsAssignableFrom<IEnumerable<Passenger>>(okResult.Value);
            Assert.Equal(2, returnedPassengers.Count());
        }

        [Fact]
        public async Task GetSurvivors_ShouldReturnOkResult_WithPaginatedSurvivors()
        {
            // Arrange
            var survivors = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe", Survived = 1 }
            };
            _mockPassengerService.Setup(service => service.GetSurvivorsAsync()).ReturnsAsync(survivors);

            // Act
            var result = await _controller.GetSurvivors(1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedSurvivors = Assert.IsAssignableFrom<IEnumerable<Passenger>>(okResult.Value);
            Assert.Single(returnedSurvivors);
        }

        [Fact]
        public async Task GetByClass_ShouldReturnOkResult_WithPassengersOfClass()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe", Pclass = 1 }
            };
            _mockPassengerService.Setup(service => service.GetByClassAsync(1)).ReturnsAsync(passengers);

            // Act
            var result = await _controller.GetByClass(1, 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPassengers = Assert.IsAssignableFrom<IEnumerable<Passenger>>(okResult.Value);
            Assert.Single(returnedPassengers);
        }

        [Fact]
        public async Task GetByGender_ShouldReturnOkResult_WithPassengersOfSpecifiedGender()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe", Sex = "male" }
            };
            _mockPassengerService.Setup(service => service.GetByGenderAsync("male")).ReturnsAsync(passengers);

            // Act
            var result = await _controller.GetByGender("male", 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPassengers = Assert.IsAssignableFrom<IEnumerable<Passenger>>(okResult.Value);
            Assert.Single(returnedPassengers);
        }

        [Fact]
        public async Task GetByAgeRange_ShouldReturnOkResult_WithPassengersInAgeRange()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe", Age = 25 }
            };
            _mockPassengerService.Setup(service => service.GetByAgeRangeAsync(20, 30)).ReturnsAsync(passengers);

            // Act
            var result = await _controller.GetByAgeRange(20, 30, 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPassengers = Assert.IsAssignableFrom<IEnumerable<Passenger>>(okResult.Value);
            Assert.Single(returnedPassengers);
        }

        [Fact]
        public async Task GetByFareRange_ShouldReturnOkResult_WithPassengersInFareRange()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe", Fare = 50 }
            };
            _mockPassengerService.Setup(service => service.GetByFareRangeAsync(30, 100)).ReturnsAsync(passengers);

            // Act
            var result = await _controller.GetByFareRange(30, 100, 1, 10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedPassengers = Assert.IsAssignableFrom<IEnumerable<Passenger>>(okResult.Value);
            Assert.Single(returnedPassengers);
        }

        [Fact]
        public async Task GetSurvivalRate_ShouldReturnOkResult_WithSurvivalRate()
        {
            // Arrange
            var survivalRate = 60.0;
            _mockPassengerService.Setup(service => service.GetSurvivalRateAsync()).ReturnsAsync(survivalRate);

            // Act
            var result = await _controller.GetSurvivalRate();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(survivalRate, okResult.Value);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtActionResult_WithCreatedPassenger()
        {
            // Arrange
            var mockService = new Mock<IPassengerService>();
            var mockMapper = new Mock<IMapper>();  // Ensure mapper is mocked

            var passengerDTO = new PassengerDTO();
            var passenger = new Passenger { Id = "123", Name = "John Doe" };

            mockMapper.Setup(m => m.Map<Passenger>(passengerDTO)).Returns(passenger);
            mockService.Setup(s => s.CreateAsync(passenger)).Returns(Task.CompletedTask);

            var controller = new PassengerController(mockService.Object, mockMapper.Object);

            // Act
            var result = await controller.Create(passengerDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetById", createdAtActionResult.ActionName);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContentResult_WhenPassengerDeleted()
        {
            // Arrange
            _mockPassengerService.Setup(service => service.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete("1");

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
