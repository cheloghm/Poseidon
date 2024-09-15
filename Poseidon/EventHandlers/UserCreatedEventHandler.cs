using Poseidon.Events;
using Poseidon.Interfaces.IEventHandlers;

namespace Poseidon.EventHandlers
{
    public class UserCreatedEventHandler : IEventHandler<UserCreatedEvent>
    {
        public Task HandleAsync(UserCreatedEvent @event)
        {
            // Logic to handle user creation (e.g., logging or sending a notification)
            Console.WriteLine($"User with ID {@event.UserId} was created at {@event.CreatedAt}");
            return Task.CompletedTask;
        }
    }
}
