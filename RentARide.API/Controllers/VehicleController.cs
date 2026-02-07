using Microsoft.AspNetCore.Mvc;
using RentARide.Application.Common;
using RentARide.Application.DTOs;
using RentARide.Application.Interfaces;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using RentARide.Infrastructure.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace RentARide.API.Controllers
{
    public class VehicleController : BaseController
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleController(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();

            var response = ApiResponse<IEnumerable<Vehicle>>.SuccessResponse(vehicles, "Vehicles retrieved successfully");

            return HandleResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);

            if (vehicle == null)
            {
                return HandleResponse(ApiResponse<Vehicle>.FailureResponse(new List<string> { "Vehicle not found" }, "Error"));
            }

            return HandleResponse(ApiResponse<Vehicle>.SuccessResponse(vehicle));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleDto dto)
        {
            var vehicle = new Vehicle
            {
                Model = dto.Model,
                Year = DateTime.SpecifyKind(dto.Year, DateTimeKind.Utc),
                LicensePlate = dto.LicensePlate,
                PricePerDay = dto.DailyPrice,
                VehicleTypeId = dto.VehicleTypeId,
                Status = VehicleStatus.Available
            };
            await _vehicleRepository.AddAsync(vehicle);
            return HandleResponse(ApiResponse<Vehicle>.SuccessResponse(vehicle, "Vehicle created successfully"));
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult>Update(Guid Id , [FromBody] UpdateVehicleDto dto )
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(Id);
            if (vehicle == null)
                return NotFound(ApiResponse<string>.FailureResponse("Vehicle not exists"));

            vehicle.Model = dto.Model;
            vehicle.PricePerDay  = dto.DailyPrice;
            vehicle.Year = DateTime.SpecifyKind(dto.Year, DateTimeKind.Utc);
            vehicle.LicensePlate = dto.LicensePlate;
            vehicle.VehicleTypeId = dto.VehicleTypeId;

            vehicle.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _vehicleRepository.UpdateAsync(vehicle);
                return HandleResponse(ApiResponse<string>.SuccessResponse("Updating Success"));
            }
            catch (Exception ex)
            {
                // هذا السطر سيجعل Swagger يخبرك بالضبط ما هي المشكلة (سواء كانت لوحة مكررة أو غيرها)
                return StatusCode(500, ApiResponse<string>.FailureResponse(ex.InnerException?.Message ?? ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>Delete(Guid id)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(id);
            if (vehicle == null)
                return NotFound(HandleResponse(ApiResponse<string>.FailureResponse("Not Found")));
            vehicle.IsDeleted = true;
            vehicle.UpdatedAt = DateTime.UtcNow;

            await _vehicleRepository.UpdateAsync(vehicle);
            return HandleResponse(ApiResponse<string>.SuccessResponse("Deleting Successed"));
        }
    }
} 