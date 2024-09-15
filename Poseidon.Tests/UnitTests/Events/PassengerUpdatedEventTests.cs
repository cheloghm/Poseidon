using Poseidon.Events;
using System;
using Xunit;

namespace Poseidon.Tests.UnitTests.Events
{
    public class PassengerUpdatedEventTests
    {
        [Fact]
        public void Constructor_SetsPassengerIdAndUpdatedAtCorrectly()
        {
            // Arrange
            var passengerId = "54321";

            // Act
            var passengerUpdatedEvent = new PassengerUpdatedEvent(passengerId);

            // Assert
            Assert.Equal(passengerId, passengerUpdatedEvent.PassengerId);
            Assert.True(passengerUpdatedEvent.UpdatedAt <= DateTime.UtcNow);
        }
    }
}
