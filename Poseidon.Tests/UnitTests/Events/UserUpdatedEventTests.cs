using Poseidon.Events;
using System;
using Xunit;

namespace Poseidon.Tests.UnitTests.Events
{
    public class UserUpdatedEventTests
    {
        [Fact]
        public void Constructor_SetsUserIdAndUpdatedAtCorrectly()
        {
            // Arrange
            var userId = "54321";

            // Act
            var userUpdatedEvent = new UserUpdatedEvent(userId);

            // Assert
            Assert.Equal(userId, userUpdatedEvent.UserId);
            Assert.True(userUpdatedEvent.UpdatedAt <= DateTime.UtcNow);
        }
    }
}
