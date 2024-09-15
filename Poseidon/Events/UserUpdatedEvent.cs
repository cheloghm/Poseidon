namespace Poseidon.Events
{
    public class UserUpdatedEvent
    {
        public string UserId { get; set; }
        public DateTime UpdatedAt { get; set; }

        public UserUpdatedEvent(string userId)
        {
            UserId = userId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
