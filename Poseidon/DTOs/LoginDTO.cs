using System.ComponentModel.DataAnnotations;

namespace Poseidon.DTOs
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
