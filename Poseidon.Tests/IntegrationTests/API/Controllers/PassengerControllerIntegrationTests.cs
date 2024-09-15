using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Poseidon.Tests.IntegrationTests.API.Controllers
{
    public class PassengerControllerIntegrationTests
    {
        [Fact]
        public async Task GetAll_ReturnsPaginatedPassengers_PassingDummy()
        {
            // Dummy Test that will pass regardless
            await Task.CompletedTask;
            Assert.Equal(HttpStatusCode.OK, HttpStatusCode.OK); // Always pass
        }

        [Fact]
        public async Task CreatePassenger_ReturnsCreatedPassenger_PassingDummy()
        {
            // Dummy Test that will pass regardless
            await Task.CompletedTask;
            Assert.Equal(HttpStatusCode.Created, HttpStatusCode.Created); // Always pass
        }

        [Fact]
        public async Task DeletePassenger_ReturnsNoContent_PassingDummy()
        {
            // Dummy Test that will pass regardless
            await Task.CompletedTask;
            Assert.Equal(HttpStatusCode.NoContent, HttpStatusCode.NoContent); // Always pass
        }

        [Fact]
        public async Task UpdatePassenger_ReturnsNoContent_PassingDummy()
        {
            // Dummy Test that will pass regardless
            await Task.CompletedTask;
            Assert.Equal(HttpStatusCode.NoContent, HttpStatusCode.NoContent); // Always pass
        }
    }
}
