using Microsoft.AspNetCore.Mvc;
using Poseidon.Interfaces.IServices;
using Poseidon.ViewModels;
using Poseidon.DTOs;
using System.Threading.Tasks;
using Poseidon.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Poseidon.Helpers;
using System.Linq;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace Poseidon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PassengerController : ControllerBase
    {
        private readonly IPassengerService _passengerService;
        private readonly IMapper _mapper;

        public PassengerController(IPassengerService passengerService, IMapper mapper)
        {
            _passengerService = passengerService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves a paginated list of all passengers.
        /// </summary>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of records per page. Defaults to 10.</param>
        /// <returns>A paginated list of passengers.</returns>
        [HttpGet("all")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.GetAllAsync();
            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }

        /// <summary>
        /// Retrieves a paginated list of passengers who survived.
        /// </summary>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of records per page. Defaults to 10.</param>
        /// <returns>A paginated list of surviving passengers.</returns>
        [HttpGet("survivors")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetSurvivors([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var survivors = await _passengerService.GetSurvivorsAsync();
            var paginatedSurvivors = PaginationHelper.Paginate(survivors.AsQueryable(), page, pageSize);
            return Ok(paginatedSurvivors);
        }

        /// <summary>
        /// Retrieves a paginated list of passengers based on their class.
        /// </summary>
        /// <param name="classNumber">The class number (1, 2, or 3) to filter passengers.</param>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of records per page. Defaults to 10.</param>
        /// <returns>A paginated list of passengers in the specified class.</returns>
        [HttpGet("class/{classNumber}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetByClass(int classNumber, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.GetByClassAsync(classNumber);
            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }

        /// <summary>
        /// Retrieves a paginated list of passengers based on gender.
        /// </summary>
        /// <param name="sex">The gender to filter passengers by (e.g., "male" or "female").</param>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of records per page. Defaults to 10.</param>
        /// <returns>A paginated list of passengers matching the specified gender.</returns>
        [HttpGet("gender/{sex}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetByGender(string sex, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.GetByGenderAsync(sex);
            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }

        /// <summary>
        /// Retrieves a paginated list of passengers within a specified age range.
        /// </summary>
        /// <param name="minAge">The minimum age to filter passengers.</param>
        /// <param name="maxAge">The maximum age to filter passengers.</param>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of records per page. Defaults to 10.</param>
        /// <returns>A paginated list of passengers within the specified age range.</returns>
        [HttpGet("age-range")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetByAgeRange([FromQuery] double minAge, [FromQuery] double maxAge, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.GetByAgeRangeAsync(minAge, maxAge);
            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }

        /// <summary>
        /// Retrieves a paginated list of passengers based on ticket fare range.
        /// </summary>
        /// <param name="minFare">The minimum fare to filter passengers.</param>
        /// <param name="maxFare">The maximum fare to filter passengers.</param>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of records per page. Defaults to 10.</param>
        /// <returns>A paginated list of passengers within the specified fare range.</returns>
        [HttpGet("fare-range")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetByFareRange([FromQuery] double minFare, [FromQuery] double maxFare, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.GetByFareRangeAsync(minFare, maxFare);
            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }

        /// <summary>
        /// Retrieves the overall survival rate of all passengers.
        /// </summary>
        /// <returns>The survival rate as a percentage.</returns>
        [HttpGet("survival-rate")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetSurvivalRate()
        {
            var survivalRate = await _passengerService.GetSurvivalRateAsync();
            return Ok(survivalRate);
        }

        /// <summary>
        /// Retrieves a specific passenger by their unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the passenger.</param>
        /// <returns>The passenger details.</returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            var passenger = await _passengerService.GetByIdAsync(id);
            return Ok(passenger);
        }

        /// <summary>
        /// Creates a new passenger and saves it to the database.
        /// </summary>
        /// <param name="passengerDTO">The passenger details.</param>
        /// <returns>The newly created passenger.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Add a new passenger", Description = "Create a new passenger. Leave the ID field blank as MongoDB will generate it automatically.")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] PassengerDTO passengerDTO)
        {
            var passenger = _mapper.Map<Passenger>(passengerDTO);
            await _passengerService.CreateAsync(passenger);
            return CreatedAtAction(nameof(GetById), new { id = passenger.Id }, passenger);
        }

        /// <summary>
        /// Updates the details of an existing passenger.
        /// </summary>
        /// <param name="id">The unique identifier of the passenger to update.</param>
        /// <param name="passengerDTO">The updated passenger details.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, [FromBody] PassengerDTO passengerDTO)
        {
            var passenger = _mapper.Map<Passenger>(passengerDTO);
            await _passengerService.UpdateAsync(id, passenger);
            return NoContent();
        }

        /// <summary>
        /// Deletes a passenger record from the database.
        /// </summary>
        /// <param name="id">The unique identifier of the passenger to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            await _passengerService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Searches for passengers based on various criteria such as name, class, sex, age range, and fare range.
        /// </summary>
        /// <param name="name">The name of the passenger to search for.</param>
        /// <param name="pclass">The class number to filter passengers.</param>
        /// <param name="sex">The gender to filter passengers.</param>
        /// <param name="minAge">The minimum age to filter passengers.</param>
        /// <param name="maxAge">The maximum age to filter passengers.</param>
        /// <param name="minFare">The minimum fare to filter passengers.</param>
        /// <param name="maxFare">The maximum fare to filter passengers.</param>
        /// <param name="page">The page number to retrieve. Defaults to 1.</param>
        /// <param name="pageSize">The number of records per page. Defaults to 10.</param>
        /// <returns>A paginated list of passengers matching the search criteria.</returns>
        [HttpGet("search")]
        [Authorize(Roles = "User, Admin")]
        [SwaggerOperation(Summary = "Search for passengers based on criteria", Description = "Allows searching passengers by name, class, sex, age range, and fare range.")]
        public async Task<IActionResult> SearchPassengers(
            [FromQuery] string name = null,
            [FromQuery] int? pclass = null,
            [FromQuery] string sex = null,
            [FromQuery] double? minAge = null,
            [FromQuery] double? maxAge = null,
            [FromQuery] double? minFare = null,
            [FromQuery] double? maxFare = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.SearchPassengersAsync(
                name, pclass, sex, minAge, maxAge, minFare, maxFare);

            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }
    }
}
