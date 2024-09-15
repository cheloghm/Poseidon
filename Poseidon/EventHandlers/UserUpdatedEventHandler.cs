using Poseidon.Events;
using Poseidon.Interfaces.IEventHandlers;

namespace Poseidon.EventHandlers
{
    public class UserUpdatedEventHandler : IEventHandler<UserUpdatedEvent>
    {
        public Task HandleAsync(UserUpdatedEvent eventMessage)
        {
            // logic for user update
            Console.WriteLine($"User updated with ID: {eventMessage.UserId} at {eventMessage.UpdatedAt}");
            return Task.CompletedTask;
        }
    }
}
