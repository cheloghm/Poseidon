// Controllers/TestController.cs

using Microsoft.AspNetCore.Mvc;

namespace PoseidonAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// GET api/test
        /// Returns a simple message to verify API integration.
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            var response = new
            {
                message = "Hello from Poseidon API Backend!"
            };

            return Ok(response); // Returns HTTP 200 with the response object
        }
    }
}
