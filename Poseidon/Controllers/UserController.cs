using Microsoft.AspNetCore.Mvc;
using Poseidon.Interfaces.IServices;
using Poseidon.DTOs;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Poseidon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Registers a new user with the specified role.
        /// </summary>
        /// <param name="createUserDTO">The details of the user to create.</param>
        /// <returns>The newly created user.</returns>
        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register a new user", Description = "Create a new user with the specified role. Role must be either 'Admin' or 'User'. Leave the ID field blank as MongoDB will generate it automatically.")]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO createUserDTO)
        {
            var createdUser = await _userService.CreateUserAsync(createUserDTO); // Now it returns the created user with Id
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }

        /// <summary>
        /// Authenticates a user and returns a JWT token upon successful login.
        /// </summary>
        /// <param name="loginDTO">The login credentials.</param>
        /// <returns>A JWT token for authenticated access.</returns>
        [HttpPost("login")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "User login", Description = "Authenticate user and receive a JWT token for authorized access.")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var token = await _userService.LoginAsync(loginDTO);
            if (token == null)
                return Unauthorized("Invalid credentials. Please use your email and password.");

            return Ok(new { Token = token });
        }

        /// <summary>
        /// Retrieves the details of a specific user by their unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user.</param>
        /// <returns>The user details.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get user details", Description = "Retrieve details of a specific user by their ID.")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Updates the details of an existing user.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="userDTO">The updated user details.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Update user details", Description = "Update information of an existing user.")]
        public async Task<IActionResult> Update(string id, [FromBody] UserDTO userDTO)
        {
            await _userService.UpdateUserAsync(id, userDTO);
            return NoContent();
        }

        /// <summary>
        /// Retrieves the details of the currently authenticated user.
        /// </summary>
        /// <returns>The user details.</returns>
        [HttpGet("me")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get current user details", Description = "Retrieve details of the currently authenticated user.")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Deletes a user account from the system.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [SwaggerOperation(Summary = "Delete a user", Description = "Remove a user account from the system.")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
