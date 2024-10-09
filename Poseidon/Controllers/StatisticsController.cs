// Controllers/StatisticsController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Poseidon.Interfaces.IServices;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;

namespace Poseidon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IPassengerService _passengerService;

        public StatisticsController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        /// <summary>
        /// Retrieves the total number of passengers.
        /// </summary>
        /// <returns>The total count of passengers.</returns>
        [HttpGet("total-passengers")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get total number of passengers", Description = "Returns the total number of passengers in the dataset.")]
        public async Task<IActionResult> GetTotalPassengers()
        {
            var total = await _passengerService.GetTotalPassengersAsync();
            return Ok(new { TotalPassengers = total });
        }

        /// <summary>
        /// Retrieves the number of male passengers.
        /// </summary>
        /// <returns>The count of male passengers.</returns>
        [HttpGet("men")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get number of men", Description = "Returns the number of male passengers.")]
        public async Task<IActionResult> GetNumberOfMen()
        {
            var menCount = await _passengerService.GetNumberOfMenAsync();
            return Ok(new { NumberOfMen = menCount });
        }

        /// <summary>
        /// Retrieves the number of female passengers.
        /// </summary>
        /// <returns>The count of female passengers.</returns>
        [HttpGet("women")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get number of women", Description = "Returns the number of female passengers.")]
        public async Task<IActionResult> GetNumberOfWomen()
        {
            var womenCount = await _passengerService.GetNumberOfWomenAsync();
            return Ok(new { NumberOfWomen = womenCount });
        }

        /// <summary>
        /// Retrieves the number of male children (boys).
        /// </summary>
        /// <returns>The count of male children.</returns>
        [HttpGet("boys")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get number of boys", Description = "Returns the number of male children passengers.")]
        public async Task<IActionResult> GetNumberOfBoys()
        {
            var boysCount = await _passengerService.GetNumberOfBoysAsync();
            return Ok(new { NumberOfBoys = boysCount });
        }

        /// <summary>
        /// Retrieves the number of female children (girls).
        /// </summary>
        /// <returns>The count of female children.</returns>
        [HttpGet("girls")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get number of girls", Description = "Returns the number of female children passengers.")]
        public async Task<IActionResult> GetNumberOfGirls()
        {
            var girlsCount = await _passengerService.GetNumberOfGirlsAsync();
            return Ok(new { NumberOfGirls = girlsCount });
        }

        /// <summary>
        /// Retrieves the number of adult passengers.
        /// </summary>
        /// <returns>The count of adult passengers.</returns>
        [HttpGet("adults")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get number of adults", Description = "Returns the number of adult passengers.")]
        public async Task<IActionResult> GetNumberOfAdults()
        {
            var adultsCount = await _passengerService.GetNumberOfAdultsAsync();
            return Ok(new { NumberOfAdults = adultsCount });
        }

        /// <summary>
        /// Retrieves the number of child passengers.
        /// </summary>
        /// <returns>The count of child passengers.</returns>
        [HttpGet("children")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get number of children", Description = "Returns the number of child passengers.")]
        public async Task<IActionResult> GetNumberOfChildren()
        {
            var childrenCount = await _passengerService.GetNumberOfChildrenAsync();
            return Ok(new { NumberOfChildren = childrenCount });
        }

        /// <summary>
        /// Retrieves the number of passengers in a specific class.
        /// </summary>
        /// <param name="classNumber">The class number to filter passengers by (1, 2, or 3).</param>
        /// <returns>The count of passengers in the specified class.</returns>
        [HttpGet("class/{classNumber}/count")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get number of passengers by class", Description = "Returns the number of passengers in the specified class.")]
        public async Task<IActionResult> GetNumberOfPassengersByClass(int classNumber)
        {
            var count = await _passengerService.GetByClassAsync(classNumber);
            return Ok(new { ClassNumber = classNumber, NumberOfPassengers = count?.Count() ?? 0 });
        }

        /// <summary>
        /// Retrieves the survival rate within a specified age range.
        /// </summary>
        /// <param name="minAge">The minimum age of the range.</param>
        /// <param name="maxAge">The maximum age of the range.</param>
        /// <returns>The survival rate as a percentage.</returns>
        [HttpGet("survival-rate/age-range")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get survival rate by age range", Description = "Returns the survival rate for passengers within the specified age range.")]
        public async Task<IActionResult> GetSurvivalRateByAgeRange([FromQuery] double minAge, [FromQuery] double maxAge)
        {
            var survivalRate = await _passengerService.GetSurvivalRateByAgeRangeAsync(minAge, maxAge);
            return Ok(new { MinAge = minAge, MaxAge = maxAge, SurvivalRate = survivalRate });
        }

        /// <summary>
        /// Retrieves the survival rate based on gender.
        /// </summary>
        /// <param name="sex">The gender to filter passengers by (e.g., "male" or "female").</param>
        /// <returns>The survival rate as a percentage.</returns>
        [HttpGet("survival-rate/gender/{sex}")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get survival rate by gender", Description = "Returns the survival rate for passengers of the specified gender.")]
        public async Task<IActionResult> GetSurvivalRateByGender(string sex)
        {
            var survivalRate = await _passengerService.GetSurvivalRateByGenderAsync(sex);
            return Ok(new { Gender = sex, SurvivalRate = survivalRate });
        }

        /// <summary>
        /// Retrieves the survival rate based on passenger class.
        /// </summary>
        /// <param name="classNumber">The class number to filter passengers by (1, 2, or 3).</param>
        /// <returns>The survival rate as a percentage.</returns>
        [HttpGet("survival-rate/class/{classNumber}")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Get survival rate by class", Description = "Returns the survival rate for passengers in the specified class.")]
        public async Task<IActionResult> GetSurvivalRateByClass(int classNumber)
        {
            var survivalRate = await _passengerService.GetSurvivalRateByClassAsync(classNumber);
            return Ok(new { ClassNumber = classNumber, SurvivalRate = survivalRate });
        }

        // Additional statistical endpoints can be added here following the same pattern.
    }
}
