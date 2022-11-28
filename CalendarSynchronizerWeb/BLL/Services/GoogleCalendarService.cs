using BLL.Intrfaces;
using BLL.Managers.Interfaces;
using Core.Models;
using Google;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;


namespace BLL.Services
{
    public class GoogleCalendarService :IGoogleCalendarService
    {
        private readonly IConfigurationManager<GoogleAuthCreds> _configurationManager;
        private CalendarService _service;
        public GoogleCalendarService(IConfigurationManager<GoogleAuthCreds> configurationManager)
        {
            _configurationManager = configurationManager;
            _service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = new CustomUserCredential(_configurationManager.Value.AccessToken),
                ApplicationName = "AppName"
            });

        }
        public async Task<CalendarList> GetAllCalendarListAsync()
        {

            try
            {
                return await _service.CalendarList.List().ExecuteAsync();
            }
            catch (GoogleApiException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                //Some Exception handling logic
                throw ex;

            }
        }

        public async Task<Events> GetCalendarEventsList(string calendarId = "primary")
        {
            try
            {
                var request = _service.Events.List(calendarId);
                var res = await request.ExecuteAsync();
                return res;
            }
            catch (GoogleApiException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                //Some Exception handling logic
                throw ex;
            }
        }
        public CalendarService GetCalenderService()
        {
            return _service;
        }
    }
}
