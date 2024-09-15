using System.ComponentModel.DataAnnotations;

namespace Poseidon.DTOs
{
    public class CreateUserDTO
    {
        public string Id { get; set; }
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User"; // Default role is User unless specified
    }
}
