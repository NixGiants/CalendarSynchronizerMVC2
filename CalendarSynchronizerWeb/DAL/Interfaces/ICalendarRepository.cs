using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICalendarRepository
    {
        public Task Create(CalendarMy calendar);
        public Task<List<CalendarMy>> GetAll();

        public Task<List<CalendarMy>> GetByUserId(string userId);

        public Task<CalendarMy> Get(string calendarId);

        public Task Delete(string id);

        public Task Update(CalendarMy calendar, string id);
    }
}
