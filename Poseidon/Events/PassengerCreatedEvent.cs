namespace Poseidon.Events
{
    public class PassengerCreatedEvent
    {
        public string PassengerId { get; set; }
        public DateTime CreatedAt { get; set; }

        public PassengerCreatedEvent(string passengerId)
        {
            PassengerId = passengerId;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
