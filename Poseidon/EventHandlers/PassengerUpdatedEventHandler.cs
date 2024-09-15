using Poseidon.Events;
using Poseidon.Interfaces.IEventHandlers;

namespace Poseidon.EventHandlers
{
    public class PassengerUpdatedEventHandler : IEventHandler<PassengerUpdatedEvent>
    {
        public Task HandleAsync(PassengerUpdatedEvent @event)
        {
            // Logic to handle passenger updates
            Console.WriteLine($"Passenger with ID {@event.PassengerId} was updated at {@event.UpdatedAt}");
            return Task.CompletedTask;
        }
    }
}
