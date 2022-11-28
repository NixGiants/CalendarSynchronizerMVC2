using BLL.Intrfaces;
using CalendarSynchronizerWeb.ViewModels.Calendar;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CalendarSynchronizerWeb.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index(string searchString)
        {
            try
            {
                var calendars = await calendarService.GetAll(searchString);

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

        [HttpGet]
        public async Task<IActionResult> SingleUserIndex(string searchString)
        {
            try
            {
                var userCalendars = await calendarService.GetByUserId(userManager.GetUserId(User), searchString);

                if (userCalendars == null)
                {
                    return View("Error");
                }

                return View(userCalendars);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return View("Error");
            }
        }

        // GET: CalendarController/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CalendarViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CalendarMy calendar = new CalendarMy
                    {
                        Summary = viewModel.Summary,
                        Description = viewModel.Description == null ? "" : viewModel.Description,
                        TimeZone = viewModel.TimeZone == null ? "" : viewModel.TimeZone,
                        AppUserId = userManager.GetUserId(User)
                    };

                    await calendarService.Create(calendar);
                    return RedirectToAction(nameof(SingleUserIndex));
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    return View("Error");

                }
            }
            return View(viewModel);
        }

        // GET: CalendarController/Edit/5
        public async Task<IActionResult> Edit(string calendarId)
        {
            var calendar = await calendarService.Get(calendarId);

            CalendarUpdateViewModel viewModel = new CalendarUpdateViewModel
            {
                CalendarId = calendar!.CalendarId,
                Summary = calendar.Summary,
                Description = calendar.Description,
                TimeZone = calendar.TimeZone
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CalendarUpdateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CalendarMy calendar = new CalendarMy
                    {
                        CalendarId = viewModel.CalendarId,
                        Summary = viewModel.Summary,
                        Description = viewModel.Description == null ? "" : viewModel.Description,
                        TimeZone = viewModel.TimeZone == null ? "" : viewModel.TimeZone
                    };

                    await calendarService.Update(calendar, calendar.CalendarId);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    return View();
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string calendarId)
        {
            try
            {
                await calendarService.Delete(calendarId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return View();
            }
        }

        public IActionResult ScheduleRedirect(string calendarId)
        {
            return RedirectToAction("Index", "Schedule", new {calendarId = calendarId});
        }
    }
}
