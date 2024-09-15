using Poseidon.Events;

namespace Poseidon.EventHandlers
{
    public class PassengerCreatedEventHandler
    {
        public Task Handle(PassengerCreatedEvent @event)
        {
            // logic for passenger creation
            Console.WriteLine($"Passenger created with ID: {@event.PassengerId} at {@event.CreatedAt}");
            return Task.CompletedTask;
        }
    }
}
