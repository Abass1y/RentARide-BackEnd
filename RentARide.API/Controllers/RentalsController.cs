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
            // 1. استخراج ID المستخدم من التوكن (لا تثق بالـ Client!)
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);

            // 2. التحقق من توفر السيارة
            if (!await _rentalService.IsVehicleAvailable(dto.VehicleId, dto.StartDate, dto.EndDate))
            {
                return BadRequest(ApiResponse<string>.FailureResponse("Vehicle is already booked for these dates."));
            }

            // 3. حساب السعر
            var totalPrice = await _rentalService.CalculateTotalPrice(dto.VehicleId, dto.StartDate, dto.EndDate);

            // 4. إنشاء الحجز
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
    }
}

