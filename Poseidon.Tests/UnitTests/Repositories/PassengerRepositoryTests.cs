using Xunit;
using Moq;
using Poseidon.Data;
using Poseidon.Repositories;
using Poseidon.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Linq;
using Poseidon.Interfaces;

namespace Poseidon.Tests.UnitTests.Repositories
{
    public class PassengerRepositoryTests
    {
        private readonly Mock<IMongoCollection<Passenger>> _mockCollection;
        private readonly Mock<IPoseidonContext> _mockContext;
        private readonly PassengerRepository _repository;

        public PassengerRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<Passenger>>();
            _mockContext = new Mock<IPoseidonContext>();

            _mockContext.Setup(c => c.Passengers).Returns(_mockCollection.Object);

            _repository = new PassengerRepository(_mockContext.Object);
        }

        [Fact]
        public async Task GetByClassAsync_ShouldReturnPassengersOfSpecifiedClass()
        {
            // Arrange
            var classNumber = 1;
            var expectedPassengers = new List<Passenger>
            {
                new Passenger { Id = "1", Name = "John Doe", Pclass = 1 },
                new Passenger { Id = "2", Name = "Jane Smith", Pclass = 1 }
            };

            var mockCursor = new Mock<IAsyncCursor<Passenger>>();
            mockCursor.Setup(_ => _.Current).Returns(expectedPassengers);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
                .Returns(true)
                .Returns(false);
            mockCursor
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            _mockCollection.Setup(c => c.FindAsync(
                It.IsAny<FilterDefinition<Passenger>>(),
                It.IsAny<FindOptions<Passenger, Passenger>>(),
                It.IsAny<System.Threading.CancellationToken>()))
                .ReturnsAsync(mockCursor.Object);

            // Act
            var result = await _repository.GetByClassAsync(classNumber);

            // Assert
            Assert.Equal(expectedPassengers.Count, result.Count());
            Assert.All(result, p => Assert.Equal(classNumber, p.Pclass));
        }
    }
}
