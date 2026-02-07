using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RentARide.Application.DTOs;

namespace RentARide.Application.Validators
{
 public class CreateVehicleValidator : AbstractValidator<CreateVehicleDto>
      {
     public CreateVehicleValidator()
     {
          RuleFor(v => v.Model) 
          .NotEmpty().WithMessage("Vehicle model is required.")
          .MinimumLength(3).WithMessage("Vehicle model must be at least 3 characters long.");

         RuleFor(v => v.DailyPrice)
          .GreaterThan(0).WithMessage("Daily price must be greater than 0.");

        RuleFor(v => v.Year)
        .NotEmpty().WithMessage("Manufacturing year is required.")
        .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Manufacturing year cannot be in the future.");

        RuleFor(v => v.LicensePlate)
        .NotEmpty().WithMessage("License plate is required.")
        .Matches(@"^[A-Z0-9- ]+$").WithMessage("License plate must contain only uppercase letters, numbers, and hyphens.");

       RuleFor(v => v.VehicleTypeId)
       .NotEmpty().WithMessage("Vehicle type must be selected.");
     }
  }
}
