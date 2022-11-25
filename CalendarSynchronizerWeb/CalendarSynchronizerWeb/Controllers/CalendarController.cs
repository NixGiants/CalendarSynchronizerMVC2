﻿using BLL.Intrfaces;
using BLL.Services;
using CalendarSynchronizerWeb.ViewModels.Calendar;
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
        public async Task<IActionResult> Index()
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

        [HttpGet]
        public async Task<IActionResult> SingleUserIndex()
        {
            try
            {
                var userCalendars = await calendarService.GetByUserId(userManager.GetUserId(User));

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
                    Calendar calendar = new Calendar
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
                    return View();

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
                    Calendar calendar = new Calendar
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
    }
}
