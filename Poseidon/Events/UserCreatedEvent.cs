namespace Poseidon.Events
{
    public class UserCreatedEvent
    {
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserCreatedEvent(string userId)
        {
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
