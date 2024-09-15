using Poseidon.Events;

namespace Poseidon.EventHandlers
{
    public class UserUpdatedEventHandler
    {
        public Task Handle(UserUpdatedEvent @event)
        {
            // logic for user update
            Console.WriteLine($"User with ID: {@event.UserId} was updated at {@event.UpdatedAt}");
            return Task.CompletedTask;
        }
    }
}
