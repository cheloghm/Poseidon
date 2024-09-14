namespace Poseidon.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // This can be used for both update and display.
    }
}
