using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens; // ضروري لـ SymmetricSecurityKey
using RentARide.Application.Common;
using RentARide.Application.DTOs;
using RentARide.Domain;
using RentARide.Domain.Entities;
using RentARide.Domain.Enums;
using RentARide.Application.Interfaces; // تأكد من المسار الصحيح للـ Interface
using System.IdentityModel.Tokens.Jwt; // ضروري لـ JwtSecurityToken
using System.Security.Claims;
using System.Text;

namespace RentARide.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // ✅ قمنا بإضافة IConfiguration هنا لكي نتمكن من قراءة appsettings.json
    public class AuthController(IUserRepository _repository, IConfiguration _configuration) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            var existingUser = await _repository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return BadRequest(ApiResponse<string>.FailureResponse("This email is already registered."));
            }

            string hashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            Users newUser = new()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                PasswordHash = hashPassword,
                UserRole = UserRole.Customer
            };

            await _repository.AddAsync(newUser);
            return Ok(ApiResponse<string>.SuccessResponse(string.Empty, "User registered successfully!"));
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _repository.GetByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized(ApiResponse<string>.FailureResponse("Invalid Email or Password."));
            }

            // ✅ استدعاء ميثود توليد التوكن
            var token = GenerateJwtToken(user);

            return Ok(ApiResponse<string>.SuccessResponse(token, "Login Successful! Welcome " + user.FirstName));
        }

        // ✅ تم نقل الميثود لتكون داخل الكلاس وليس خارجه
        private string GenerateJwtToken(Users user)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is missing!");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),
                new Claim("FirstName", user.FirstName)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:DurationInMinutes"] ?? "60")),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}