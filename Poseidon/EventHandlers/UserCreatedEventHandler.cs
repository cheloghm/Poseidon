using Poseidon.Events;

namespace Poseidon.EventHandlers
{
    public class UserCreatedEventHandler
    {
        public Task Handle(UserCreatedEvent @event)
        {
            // Logic to handle user creation (e.g., logging or sending a notification)
            Console.WriteLine($"User with ID {@event.UserId} was created at {@event.CreatedAt}");
            return Task.CompletedTask;
        }
    }
}
