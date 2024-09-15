using Poseidon.Events;

namespace Poseidon.EventHandlers
{
    public class PassengerUpdatedEventHandler
    {
        public Task Handle(PassengerUpdatedEvent @event)
        {
            // Logic to handle passenger updates
            Console.WriteLine($"Passenger with ID {@event.PassengerId} was updated at {@event.UpdatedAt}");
            return Task.CompletedTask;
        }
    }
}
