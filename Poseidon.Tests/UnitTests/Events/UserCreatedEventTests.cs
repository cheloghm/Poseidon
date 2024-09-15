using Poseidon.Events;
using System;
using Xunit;

namespace Poseidon.Tests.UnitTests.Events
{
    public class UserCreatedEventTests
    {
        [Fact]
        public void Constructor_SetsUserIdAndCreatedAtCorrectly()
        {
            // Arrange
            var userId = "12345";

            // Act
            var userCreatedEvent = new UserCreatedEvent(userId);

            // Assert
            Assert.Equal(userId, userCreatedEvent.UserId);
            Assert.True(userCreatedEvent.CreatedAt <= DateTime.UtcNow);
        }
    }
}
