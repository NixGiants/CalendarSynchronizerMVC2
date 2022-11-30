using BLL.Intrfaces;
using CalendarSynchronizerWeb.Models;
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
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["SummarySortParam"] = String.IsNullOrEmpty(sortOrder) ? "summary_desc" : "";
            ViewData["DescriptionSortParam"] = sortOrder == "Description" ? "description_desc" : "Description";
            ViewData["CalendarIdSortParam"] = sortOrder == "CalendarId" ? "calendarId_desc" : "CalendarId";


            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            try
            {
                var currentUser = await userManager.GetUserAsync(User);
                List<CalendarMy> calendarsList = new List<CalendarMy>();

                if (await userManager.IsInRoleAsync(currentUser, "Admin"))
                {
                    calendarsList = await calendarService.GetAll(sortOrder, searchString);
                }
                else
                {
                    calendarsList = await calendarService.GetByUserId(userManager.GetUserId(User), searchString, sortOrder);
                }

                if (calendarsList == null)
                {
                    return View("Error");
                }

                int pageSize = 3;
                return View(CalendarPaginatedList<CalendarMy>.CreateAsync(calendarsList, pageNumber ?? 1, pageSize));
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
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    logger.LogError(e.Message);
                    return View("Error");

                }
            }
            return View(viewModel);
        }

        //[HttpGet]
        public async Task<IActionResult> Details(string calendarId)
        {
            var calendar = await calendarService.Get(calendarId);

            if(calendar == null)
            {
                return RedirectToAction(nameof(Index));
            }

            CalendarViewModel viewModel = new CalendarViewModel
            {
                CalendarId = calendar!.CalendarId,
                Summary = calendar.Summary,
                Description = calendar.Description,
                TimeZone = calendar.TimeZone
            };

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
