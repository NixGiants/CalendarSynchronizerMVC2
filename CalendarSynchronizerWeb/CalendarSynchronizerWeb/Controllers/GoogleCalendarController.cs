using CalendarSynchronizerWeb.Managers.Interfaces;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Mvc;

namespace CalendarSynchronizerWeb.Controllers
{
    public class GoogleCalendarController : Controller
    {
        private readonly IConfigurationManager<GoogleAuthCreds> _configurationManager;
        private readonly ApplicationDbContext _context;
        public GoogleCalendarController(IConfigurationManager<GoogleAuthCreds> configurationManager, ApplicationDbContext context)
        {
            _configurationManager = configurationManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }



    }
}
