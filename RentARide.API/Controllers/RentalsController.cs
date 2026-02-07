using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentARide.Application.Common;
using RentARide.Application.DTOs;
using RentARide.Application.Services;
using RentARide.Domain.Entities;
using RentARide.Domain.interfaces;

namespace RentARide.API.Controllers
{
    [Authorize] // حماية كاملة
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController(RentalService _rentalService, IRentalRepository _rentalRepository) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRentalDto dto)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            if (!await _rentalService.IsVehicleAvailable(dto.VehicleId, dto.StartDate, dto.EndDate))
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Vehicle is already booked for these dates."));
            }

            var totalPrice = await _rentalService.CalculateTotalPrice(dto.VehicleId, dto.StartDate, dto.EndDate);

            var rental = new Rental
            {
                UserId = userId,
                VehicleId = dto.VehicleId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalPrice = totalPrice,
                Status = "Active"
            };

            await _rentalRepository.AddAsync(rental);

            return Ok(ApiResponse<object>.SuccessResponse(new { TotalPrice = totalPrice }, "Rental created successfully!"));
        }
        [HttpGet("my-rentals")]
        public async Task<IActionResult> GetMyRentals()
        {
            
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
                return Unauthorized(ApiResponse<object>.FailureResponse(new List<string> { "User ID not found in token" }));

            var userId = Guid.Parse(userIdClaim.Value);

            var rentals = await _rentalRepository.GetRentalsByUserIdAsync(userId);

            var result = rentals.Select(r => new {
                r.Id,
                r.StartDate,
                r.EndDate,
                r.TotalPrice,
                VehicleModel = r.Vehicle.Model,
                r.Vehicle.LicensePlate
            });

            return Ok(ApiResponse<object>.SuccessResponse(result, "Your rentals retrieved successfully"));
        }
    }
}

