using CalendarSynchronizerWeb.Managers.Interfaces;
using CalendarSynchronizerWeb.Models;
using Core.Models;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;

namespace CalendarSynchronizerWeb.Controllers
{
    public class CalendarsController : Controller
    {
        static HttpClient httpClient = new HttpClient();
        private readonly IConfigurationManager<GoogleAuthCreds> _configurationManager;

        public CalendarsController(IConfigurationManager<GoogleAuthCreds> configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public async Task<IActionResult> Index()
        {
            await getCalaendars();
            return View();
        }
        public async Task<Calendar> getCalaendars()
        {
            CalendarService service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = new CustomUserCredential(_configurationManager.Value.AccessToken),
                ApplicationName = "AppName"
            });

            CalendarList list = await service.CalendarList.List().ExecuteAsync();

            string calendarId = list.Items.Where(c => c.Summary == "dgekichan2014@gmail.com").ToList()[0].Id;
            var res = await service.Calendars.Get(calendarId).ExecuteAsync();


            Event newEvent = new Event()
            {
                Summary = "Google I/O 2015",
                Location = "800 Howard St., San Francisco, CA 94103",
                Description = "A chance to hear more about Google's developer products.",
                Start = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2022-11-23T09:00:00-07:00"),
                    TimeZone = "America/Los_Angeles",
                },
                End = new EventDateTime()
                {
                    DateTime = DateTime.Parse("2022-11-24T17:00:00-07:00"),
                    TimeZone = "America/Los_Angeles",
                },
                Recurrence = new String[] { "RRULE:FREQ=DAILY;COUNT=2" },
                Reminders = new Event.RemindersData()
                {
                    UseDefault = false,
                    Overrides = new EventReminder[] {
                        new EventReminder() { Method = "email", Minutes = 24 * 60 }
                    }
                }
            };

            EventsResource.InsertRequest request = service.Events.Insert(newEvent, calendarId);
            Event createdEvent = request.Execute();


            //Calendar calendarBody = new Calendar();
            //calendarBody.Summary = "MyTestCalendar";
            //var resInsert = await service.Calendars.Insert(calendarBody).ExecuteAsync();

            Uri uri = new Uri("https://www.googleapis.com/calendar/v3/users/me/calendarList");

            //HttpMethod method = HttpMethod.Get;
            //HttpRequestMessage message = new HttpRequestMessage(method, uri);
            //message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _configurationManager.Value.AccessToken);

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _configurationManager.Value.AccessToken);
            var result = await httpClient.GetAsync(uri);
            Console.WriteLine(result);
            return res;
        }

    }
}
