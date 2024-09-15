using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Poseidon.Interfaces;
using Poseidon.Services;
using Xunit;

namespace Poseidon.Tests.UnitTests.Services
{
    public class TokenServiceTests
    {
        private readonly Mock<ITokenRepository> _tokenRepositoryMock;
        private readonly Mock<ILogger<TokenService>> _loggerMock;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _tokenRepositoryMock = new Mock<ITokenRepository>();
            _loggerMock = new Mock<ILogger<TokenService>>();
            _tokenService = new TokenService(_tokenRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CleanupExpiredTokens_LogsInformationAndRemovesExpiredTokens()
        {
            // Arrange
            var removedCount = 5;
            _tokenRepositoryMock.Setup(tr => tr.RemoveExpiredTokens()).ReturnsAsync(removedCount);

            // Act
            await _tokenService.CleanupExpiredTokens();

            // Assert
            _tokenRepositoryMock.Verify(tr => tr.RemoveExpiredTokens(), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Starting cleanup of expired tokens")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains($"{removedCount} expired tokens were successfully removed")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task CleanupExpiredTokens_LogsErrorIfExceptionOccurs()
        {
            // Arrange
            var exception = new Exception("Test exception");
            _tokenRepositoryMock.Setup(tr => tr.RemoveExpiredTokens()).ThrowsAsync(exception);

            // Act
            await _tokenService.CleanupExpiredTokens();

            // Assert
            _tokenRepositoryMock.Verify(tr => tr.RemoveExpiredTokens(), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("An error occurred while cleaning up expired tokens")),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
