using BLL.Intrfaces;
using CalendarSynchronizerWeb.Models;
using CalendarSynchronizerWeb.ViewModels.Schedule;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace CalendarSynchronizerWeb.Controllers
{
    [Authorize(Roles = "User, Admin")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService scheduleService;
        private readonly ILogger<ScheduleController> logger;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ScheduleController(ILogger<ScheduleController> logger, IScheduleService scheduleService, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.scheduleService = scheduleService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(string calendarId, string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["StartTimeSortParam"] = String.IsNullOrEmpty(sortOrder) ? "startTime_desc" : "";
            ViewData["EndTimeSortParam"] = sortOrder == "EndTime" ? "endTime_desc" : "EndTime";

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
                if (calendarId == null)
                {
                    httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("calendarId", out calendarId);
                }
                else
                {
                    httpContextAccessor.HttpContext.Response.Cookies.Append("calendarId", calendarId);
                }
                var schedules = await scheduleService.GetCalendarSchedules(calendarId, searchString, sortOrder);

                int pageSize = 3;
                return View(CalendarPaginatedList<Schedule>.CreateAsync(schedules, pageNumber ?? 1, pageSize));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return View("Error");
            }
        }

        // GET: ScheduleController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ScheduleController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScheduleController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ScheduleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string? id;
                    httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("calendarId", out id);
                    if (id == null)
                    {
                        return View("Error");
                    }

                    var schedule = transfromViewModelToModel(viewModel);
                    schedule.CalendarId = id;
                    await scheduleService.Create(schedule);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message);
                    return View();
                }
            }

            return View(viewModel);

        }

        // GET: ScheduleController/Edit/5
        public async Task<IActionResult> Edit(Guid scheduleId)
        {
            var schedule = await scheduleService.GetSchedule(scheduleId);

            if (schedule == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = transfromModelToViewModel(schedule);

            return View(viewModel);
        }

        // POST: ScheduleController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ScheduleUpdateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string? id;
                    httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("calendarId", out id);
                    var schedule = transfromViewModelToModel(viewModel);

                    if (id == null)
                    {
                        return Redirect("Error");
                    }

                    schedule.CalendarId = id;
                    await scheduleService.Update(schedule.Id, schedule);
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

        // POST: ScheduleController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid scheduleId)
        {
            try
            {
                await scheduleService.Delete(scheduleId);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return View();
            }
        }

        private Schedule transfromViewModelToModel(ScheduleViewModel viewModel)
        {
            return new Schedule
            {
                Subject = viewModel.Subject ?? "none",
                StartTime = viewModel.StartTime,
                EndTime = viewModel.EndTime,
                StartTimezone = viewModel.EndTimeZone ?? "none",
                EndTimezone = viewModel.EndTimeZone ?? "none",
                Status = viewModel.Status ?? "none",
                Description = viewModel.Description ?? "none",
                Location = viewModel.Location ?? "none",
                IsAllDay = viewModel.IsAllDay,
            };
        }

        private Schedule transfromViewModelToModel(ScheduleUpdateViewModel viewModel)
        {
            return new Schedule
            {
                Id = viewModel.Id,
                Subject = viewModel.Subject,
                StartTime = viewModel.StartTime,
                EndTime = viewModel.EndTime,
                StartTimezone = viewModel.EndTimeZone,
                EndTimezone = viewModel.EndTimeZone,
                Status = viewModel.Status,
                Description = viewModel.Description,
                Location = viewModel.Location,
                IsAllDay = viewModel.IsAllDay
            };
        }

        private ScheduleViewModel transfromModelToViewModel(Schedule model)
        {
            return new ScheduleViewModel
            {
                Id = model.Id,
                Subject = model.Subject,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                StartTimeZone = model.StartTimezone,
                EndTimeZone = model.EndTimezone,
                Status = model.Status,
                Description = model.Description,
                Location = model.Location,
                IsAllDay = model.IsAllDay,
                CalendarId = model.CalendarId
            };
        }
    }
}
