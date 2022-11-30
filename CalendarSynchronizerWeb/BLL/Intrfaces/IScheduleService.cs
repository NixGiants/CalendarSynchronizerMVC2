using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Intrfaces
{
    public interface IScheduleService
    {
        public Task Create(Schedule schedule);
        public Task<Schedule?> GetSchedule(Guid id);
        public Task<List<Schedule>> GetCalendarSchedules(string calendarId, string? searchString ,string sortOrder);
        public Task Delete(Guid id);

        public Task Update(Guid id, Schedule schedule);
    }
}
