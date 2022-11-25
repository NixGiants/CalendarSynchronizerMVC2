using BLL.Intrfaces;
using BLL.Services;
using CalendarSynchronizerWeb.ViewModels;
using Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarSynchronizerWeb.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ICalendarService calendarService;
        private readonly UserManager<AppUser> userManager;
        private readonly ILogger<CalendarController> logger;

        public CalendarController(ICalendarService calendarService, ILogger<CalendarController> logger, UserManager<AppUser> userManager)
        {
            this.calendarService = calendarService;
            this.logger = logger;
            this.userManager = userManager;
        }

        // GET: CalendarController
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                var calendars = await calendarService.GetAll();

                if (calendars == null)
                {
                    return View("Error");
                }

                return View(calendars);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return View("Error");
            }
        }

        // GET: CalendarController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CalendarController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CalendarViewModel viewModel)
        {
            try
            {
                Calendar calendar = new Calendar
                {
                    Summary = viewModel.Summary,
                    Description = viewModel.Description,
                    TimeZone = viewModel.TimeZone,
                    AppUserId = userManager.GetUserId(User)
                };
                await calendarService.Create(calendar);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
                return View();

            }
        }

        // GET: CalendarController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CalendarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CalendarController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CalendarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
