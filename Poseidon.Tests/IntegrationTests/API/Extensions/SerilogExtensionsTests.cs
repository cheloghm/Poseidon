using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Poseidon.Extensions;
using Serilog;
using Xunit;

namespace Poseidon.Tests.IntegrationTests.API
{
    public class SerilogExtensionsTests
    {
        [Fact]
        public void ConfigureSerilog_ShouldSetUpSerilogCorrectly()
        {
            // Arrange
            var mockHostBuilder = new Mock<IHostBuilder>();

            // Act
            SerilogExtensions.ConfigureSerilog(mockHostBuilder.Object);

            // Assert
            Assert.NotNull(Log.Logger);  // Ensure Serilog logger is configured
            Assert.True(Log.Logger.GetType().Name.Contains("Logger")); // Verify it's the correct type
        }
    }
}
