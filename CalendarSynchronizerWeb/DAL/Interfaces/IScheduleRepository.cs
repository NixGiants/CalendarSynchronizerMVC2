using Core.Models;

namespace DAL.Interfaces
{
    public interface IScheduleRepository
    {
        public Task Create(Schedule schedule);
        public Task<Schedule> GetSchedule(Guid id);
        public Task<List<Schedule>> GetCalendarSchedules(string calendarId);

        public Task Delete(Guid id);

        public Task Update(Guid id, Schedule schedule);
    }
}
