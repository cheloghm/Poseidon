using Moq;
using MongoDB.Driver;
using Poseidon.Data;
using Poseidon.Interfaces.IRepositories;
using Poseidon.Models;
using Poseidon.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Options;
using Poseidon.Config;

namespace Poseidon.Tests.UnitTests.Repositories
{
    public class PassengerRepositoryTests
    {
        private readonly PassengerRepository _repository;
        private readonly Mock<IMongoCollection<Passenger>> _mockPassengerCollection;

        public PassengerRepositoryTests()
        {
            // Mock the MongoDB collection
            _mockPassengerCollection = new Mock<IMongoCollection<Passenger>>();

            // Mock the IOptions<DatabaseConfig>
            var mockConfig = new Mock<IOptions<DatabaseConfig>>();
            mockConfig.SetupGet(c => c.Value).Returns(new DatabaseConfig
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "PoseidonDB"
            });

            // Create a mock context by directly mocking the Passengers collection
            var mockContext = new Mock<PoseidonContext>(mockConfig.Object);
            mockContext.Setup(c => c.Passengers).Returns(_mockPassengerCollection.Object);

            // Create the repository with the mock context
            _repository = new PassengerRepository(mockContext.Object);
        }

        [Fact]
        public async Task GetByClassAsync_ShouldReturnPassengersOfSpecifiedClass()
        {
            // Arrange
            var passengers = new List<Passenger>
            {
                new Passenger { Id = "1", Pclass = 1, Name = "John Doe" },
                new Passenger { Id = "2", Pclass = 1, Name = "Jane Doe" }
            };

            // Mocking IAsyncCursor to simulate MongoDB cursor behavior
            var asyncCursorMock = new Mock<IAsyncCursor<Passenger>>();
            asyncCursorMock.Setup(_ => _.Current).Returns(passengers);
            asyncCursorMock
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            asyncCursorMock
                .SetupSequence(_ => _.MoveNextAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            // Mock the Find operation to return the mocked cursor
            _mockPassengerCollection
                .Setup(c => c.FindAsync(It.IsAny<FilterDefinition<Passenger>>(),
                                        It.IsAny<FindOptions<Passenger>>(),
                                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(asyncCursorMock.Object);

            // Act
            var result = await _repository.GetByClassAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
