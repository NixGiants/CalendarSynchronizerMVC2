using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;


namespace BLL.Intrfaces
{
    public interface IGoogleCalendarService
    {
        public Task<CalendarList> GetAllCalendarListAsync();

        public Task<Events> GetCalendarEventsList(string calendarId = "primary");
        public CalendarService GetCalenderService();

    }
}
