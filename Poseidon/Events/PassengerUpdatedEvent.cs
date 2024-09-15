namespace Poseidon.Events
{
    public class PassengerUpdatedEvent
    {
        public string PassengerId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public PassengerUpdatedEvent(string passengerId)
        {
            PassengerId = passengerId;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
