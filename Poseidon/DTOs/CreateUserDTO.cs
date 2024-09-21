using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Poseidon.DTOs
{
    public class CreateUserDTO
    {
        // Hide this field in Swagger for POST operations
        [SwaggerSchema(ReadOnly = true, Description = "Leave this field blank; MongoDB will automatically generate the ID.")]
        public string Id { get; set; }
        public string Username { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [RegularExpression("^(Admin|User)$", ErrorMessage = "Role must be either 'Admin' or 'User'")]
        [SwaggerSchema(Description = "Role must be either 'Admin' or 'User'")]
        public string Role { get; set; } = "User"; // Default role is User unless specified
    }
}
