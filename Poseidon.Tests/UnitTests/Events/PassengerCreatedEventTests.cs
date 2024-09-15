using Poseidon.Events;
using System;
using Xunit;

namespace Poseidon.Tests.UnitTests.Events
{
    public class PassengerCreatedEventTests
    {
        [Fact]
        public void Constructor_SetsPassengerIdAndCreatedAtCorrectly()
        {
            // Arrange
            var passengerId = "12345";

            // Act
            var passengerCreatedEvent = new PassengerCreatedEvent(passengerId);

            // Assert
            Assert.Equal(passengerId, passengerCreatedEvent.PassengerId);
            Assert.True(passengerCreatedEvent.CreatedAt <= DateTime.UtcNow);
        }
    }
}
