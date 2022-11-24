using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CalendarSynchronizerWeb.Controllers
{
    [Authorize]
    public class AccessController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AdminAccess()
        {
            return View();
        }

        [Authorize(Policy = "OnlyAdminChecker")]
        public IActionResult OnlyAdminChecker()
        {
            return View();
        }

        [Authorize(Policy = "CheckUserNameTeddy")]
        public IActionResult CheckUserNameTeddy()
        {
            return View();
        }
    }


}
