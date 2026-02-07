using Microsoft.AspNetCore.Mvc;

namespace RentARide.API.Controllers
{
    public class VehicleTypesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
