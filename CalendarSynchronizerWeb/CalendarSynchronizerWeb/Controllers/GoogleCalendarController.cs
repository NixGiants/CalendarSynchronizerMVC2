using BLL.Managers.Interfaces;
using Core.Models;
using DAL;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using CalendarSynchronizerWeb.Extensions;
using Google.Apis.Calendar.v3.Data;
using CalendarSynchronizerWeb.Services;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using CalendarSynchronizerWeb.ViewModels.Calendar;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using BLL.Intrfaces;

namespace CalendarSynchronizerWeb.Controllers
{
    public class GoogleCalendarController : Controller
    {
        private readonly IConfigurationManager<GoogleAuthCreds> _configurationManager;
        private readonly ApplicationDbContext _context;
        private readonly IRepository<Schedule> _repository;
        private string calendarId = "test.calendar.google.com";
        private readonly CalendarService calendarService;
        private readonly IGoogleCalendarService service;

        public GoogleCalendarController(IConfigurationManager<GoogleAuthCreds> configurationManager, ApplicationDbContext context, IGoogleCalendarService service)
        {
            _configurationManager = configurationManager;
            _context = context;
            this.calendarService = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = new CustomUserCredential(_configurationManager.Value.AccessToken),
                ApplicationName = "CalendarSynchronizer"
            });
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            CalendarList list = await calendarService.CalendarList.List().ExecuteAsync();
            var calendarsFromGoogle = list.Items;
            List<CalendarMy> calendars = new List<CalendarMy>();    
            foreach(var item in calendarsFromGoogle)
            {
                calendars.Add(new CalendarMy
                {
                    CalendarId = item.Id,
                    Summary = item.Summary,
                    Description = item.Description,
                    TimeZone = item.TimeZone,
                });
            }

            return View(calendars);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CalendarViewModel viewModel)
        {
            if (ModelState.IsValid) {
                Calendar newCalendar = new Calendar()
                {
                    Summary =viewModel.Summary,
                    Description=viewModel.Description,
                    TimeZone=viewModel.TimeZone,
                };
                CalendarsResource.InsertRequest request = calendarService.Calendars.Insert(newCalendar);
                Calendar calendar = request.Execute();
                return RedirectToAction(nameof(Index));
            }

            return View("Error");
        }

        public async Task InitialSync(/*calendarId param*/)
        {
            //GoogleCalendarService service = new GoogleCalendarService(_configurationManager);
            var res1 = await service.GetAllCalendarListAsync();

            var res = await service.GetCalendarEventsList(calendarId);

            _configurationManager.ChangeAppSettingValue(opt => opt.SyncToken = res.NextSyncToken);

            foreach (var r in res.Items)
            {
                if (r.Status == "cancelled")
                {
                    continue;
                }
                await _repository.CreateAsync(new Schedule
                {
                    Description = r.Description,
                    StartTime = r.Start.DateTime.HasValue ? r.Start.DateTime.Value : DateTime.Parse(r.Start.Date),
                    EndTime = r.End.DateTime.HasValue ? r.End.DateTime.Value : DateTime.Parse(r.End.Date),
                    StartTimezone = r.Start.TimeZone,
                    Status = r.Status,
                    EndTimezone = r.End.TimeZone,
                    ExternalId = r.Id,
                    Location = r.Location,
                    RecurrenceID = r.RecurringEventId,
                    RecurrenceRule = r.Recurrence is null ? String.Empty : String.Concat(r.Recurrence.SelectMany(s => s)),
                    RecurrenceException = String.IsNullOrEmpty(r.RecurringEventId) ? String.Empty : r.Start.DateTime.HasValue ? r.Start.DateTime.Value.ToString("yyyyMMddTHHmmssZ") : Convert.ToDateTime(r.Start.Date).ToString("yyyyMMddTHHmmssZ"),
                    IsAllDay = !String.IsNullOrEmpty(r.Start.Date),
                    Subject = r.Summary
                });
            }



        }
        public async Task Sync(/*calendarId param*/)
        {
            if (!await _repository.Any())
            {
                return;
            }
            //GoogleCalendarService service = new GoogleCalendarService(_configurationManager);
            var res1 = await service.GetAllCalendarListAsync();

            var res = await service.GetCalendarEventsList(calendarId);

            var fromDb = await _repository.GetAllAsync();

            var fromServer = new List<Schedule>();

            foreach (var r in res.Items)
            {
                fromServer.Add(new Schedule
                {
                    Description = r.Description,
                    StartTime = r.Start.DateTime.HasValue ? r.Start.DateTime.Value : DateTime.Parse(r.Start.Date),
                    EndTime = r.End.DateTime.HasValue ? r.End.DateTime.Value : DateTime.Parse(r.End.Date),
                    StartTimezone = r.Start.TimeZone,
                    Status = r.Status,
                    EndTimezone = r.End.TimeZone,
                    ExternalId = r.Id,
                    Location = r.Location,
                    RecurrenceID = r.RecurringEventId,
                    RecurrenceRule = r.Recurrence is null ? String.Empty : String.Concat(r.Recurrence.SelectMany(s => s)),
                    RecurrenceException = String.IsNullOrEmpty(r.RecurringEventId) ? String.Empty : r.Start.DateTime.HasValue ? r.Start.DateTime.Value.ToString("yyyyMMddTHHmmssZ") : Convert.ToDateTime(r.Start.Date).ToString("yyyyMMddTHHmmssZ"),
                    IsAllDay = !String.IsNullOrEmpty(r.Start.Date),
                    Subject = r.Summary
                });
            }

            fromDb.Sort(new ScheduleComparer());
            fromServer.Sort(new ScheduleComparer());
            //s - from google, t - from db

            var diff = fromServer.ZipEqual(fromDb, new ScheduleComparer(), (s, t) =>
            {
                if (s == null) return new ScheduleStateChanged { Target = t, Action = State.Delete };

                if (t == null) return new ScheduleStateChanged { Source = s, Action = State.Create };

                return new ScheduleStateChanged { Source = s, Target = t, Action = State.Update };
            });

            await _repository.HandleScheduleStatesAsync(diff);
            ;




        }

        public async Task IncrementalSync(/*calendarId param*/)
        {
            //GoogleCalendarService service = new GoogleCalendarService(_configurationManager);

            var calendar = service.GetCalenderService(); //TODO: refactor this trash

            var request = calendar.Events.List(calendarId);



            string pageToken = null;
            string syncToken = _configurationManager.Value.SyncToken;
            Events events = null;
            List<Event> result = new List<Event>();

            if (syncToken == "default")
            {
                //do full sync 
                ;
            }
            else
            {
                request.SyncToken = syncToken;
            }

            do
            {
                request.PageToken = pageToken;

                events = await request.ExecuteAsync();



                //syncEvent(event);


                var fromDb = await _repository.GetAllAsync();

                var fromServer = new List<Schedule>();


                foreach (var r in events.Items)
                {
                    if (r.Status == "cancelled")
                    {
                        fromServer.Add(new Schedule
                        {
                            ExternalId = r.Id,
                            RecurrenceID = r.RecurringEventId,
                            RecurrenceRule = r.Recurrence is null ? String.Empty : String.Concat(r.Recurrence.SelectMany(s => s)),
                            Status = r.Status

                        });
                    }
                    else
                    {
                        fromServer.Add(new Schedule
                        {
                            Description = r.Description,
                            StartTime = r.Start.DateTime.HasValue ? r.Start.DateTime.Value : DateTime.Parse(r.Start.Date),
                            EndTime = r.End.DateTime.HasValue ? r.End.DateTime.Value : DateTime.Parse(r.End.Date),
                            StartTimezone = r.Start.TimeZone,
                            Status = r.Status,
                            EndTimezone = r.End.TimeZone,
                            ExternalId = r.Id,
                            Location = r.Location,
                            RecurrenceID = r.RecurringEventId,
                            RecurrenceRule = r.Recurrence is null ? String.Empty : String.Concat(r.Recurrence.SelectMany(s => s)),
                            RecurrenceException = String.IsNullOrEmpty(r.RecurringEventId) ? String.Empty : r.Start.DateTime.HasValue ? r.Start.DateTime.Value.ToString("yyyyMMddTHHmmssZ") : Convert.ToDateTime(r.Start.Date).ToString("yyyyMMddTHHmmssZ"),
                            IsAllDay = !String.IsNullOrEmpty(r.Start.Date),
                            Subject = r.Summary
                        });
                    }
                    //result.Add(item);
                }


                var diff = fromServer.ZipEqual(fromDb, new ScheduleComparer(), (s, t) =>
                {
                    if (s == null) return new ScheduleStateChanged { Target = t, Action = State.Delete };

                    if (t == null) return new ScheduleStateChanged { Source = s, Action = State.Create };

                    return new ScheduleStateChanged { Source = s, Target = t, Action = State.Update };
                });

                ;

                //pageToken = events.NextPageToken;

            } while (pageToken != null);

            // Store the sync token from the last request to be used during the next execution.
            _configurationManager.ChangeAppSettingValue(opt => opt.SyncToken = events.NextSyncToken);
        }
    }
}
