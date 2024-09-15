using Poseidon.Events;
using Poseidon.Interfaces.IEventHandlers;

namespace Poseidon.EventHandlers
{
    public class PassengerCreatedEventHandler : IEventHandler<PassengerCreatedEvent>
    {
        public Task HandleAsync(PassengerCreatedEvent eventMessage)
        {
            // logic for passenger creation
            Console.WriteLine($"Passenger created with ID: {eventMessage.PassengerId} at {eventMessage.CreatedAt}");
            return Task.CompletedTask;
        }
    }
}
