using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigiMedia.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "superAdmin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
