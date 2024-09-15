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

        [HttpGet("all")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.GetAllAsync();
            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }

        [HttpGet("survivors")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetSurvivors([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var survivors = await _passengerService.GetSurvivorsAsync();
            var paginatedSurvivors = PaginationHelper.Paginate(survivors.AsQueryable(), page, pageSize);
            return Ok(paginatedSurvivors);
        }

        [HttpGet("class/{classNumber}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetByClass(int classNumber, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.GetByClassAsync(classNumber);
            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }

        [HttpGet("gender/{sex}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetByGender(string sex, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.GetByGenderAsync(sex);
            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }

        [HttpGet("age-range")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetByAgeRange([FromQuery] double minAge, [FromQuery] double maxAge, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.GetByAgeRangeAsync(minAge, maxAge);
            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }

        [HttpGet("fare-range")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetByFareRange([FromQuery] double minFare, [FromQuery] double maxFare, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var passengers = await _passengerService.GetByFareRangeAsync(minFare, maxFare);
            var paginatedPassengers = PaginationHelper.Paginate(passengers.AsQueryable(), page, pageSize);
            return Ok(paginatedPassengers);
        }

        [HttpGet("survival-rate")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetSurvivalRate()
        {
            var survivalRate = await _passengerService.GetSurvivalRateAsync();
            return Ok(survivalRate);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            var passenger = await _passengerService.GetByIdAsync(id);
            return Ok(passenger);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] PassengerDTO passengerDTO)
        {
            var passenger = _mapper.Map<Passenger>(passengerDTO);
            await _passengerService.CreateAsync(passenger);
            return CreatedAtAction(nameof(GetById), new { id = passenger.Id }, passenger);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(string id, [FromBody] PassengerDTO passengerDTO)
        {
            var passenger = _mapper.Map<Passenger>(passengerDTO);
            await _passengerService.UpdateAsync(id, passenger); 
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            await _passengerService.DeleteAsync(id);
            return NoContent();
        }
    }
}
