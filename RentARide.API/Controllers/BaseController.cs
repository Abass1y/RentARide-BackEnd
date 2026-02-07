using Microsoft.AspNetCore.Mvc;
using RentARide.Application.Common;

namespace RentARide.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController: ControllerBase
    {
        protected IActionResult HandleResponse<T>(ApiResponse<T> response)
        {
            if(response.Success)
            {
                return Ok(response); // 200
            }
            if(response.Errors !=null && response.Errors.Any())
            {
                return BadRequest(response); // 400
            }
            return NotFound(response); // 404
        }
    }
}
