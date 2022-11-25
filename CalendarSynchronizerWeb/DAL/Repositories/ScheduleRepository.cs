using Core.Models;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
   public class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ScheduleRepository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public async Task Create(Schedule schedule)
        {
            var calendar = await dbContext.Calendars.FindAsync(schedule.CalendarId);

            if (calendar == null)
            {
                throw new ArgumentNullException($"No calendars with Id {schedule.CalendarId} for calenar");
            }

            schedule.Calendar = calendar;
            await dbContext.Schedules.AddAsync(schedule);
            await dbContext.SaveChangesAsync();
            return;
        }

        public async Task Delete(Guid id)
        {
            var schedule = await dbContext.Schedules.FindAsync(id);

            if (schedule is null)
            {
                throw new ArgumentException($"No Schedule with id {id}");
            }

            dbContext.Schedules.Remove(schedule);
            await dbContext.SaveChangesAsync();
            return;
        }

        public async Task<Schedule> GetSchedule(Guid id)
        {
            var schedule = await dbContext.Schedules.FindAsync(id);

            if (schedule is null)
            {
                throw new ArgumentException($"No Schedule with id {id}");
            }

            return schedule;
        }

        public async Task<List<Schedule>> GetCalendarSchedules(string calendarId)
        {
            return await dbContext.Schedules.Include(sc => sc.Calendar).Where(sc => sc.CalendarId == calendarId).ToListAsync();
        }

        public async Task Update(Guid id, Schedule schedule)
        {
            var dbSchedule = await dbContext.Schedules.FindAsync(id);

            if (dbSchedule is null)
            {
                throw new ArgumentException($"No Schedule with id {id}");
            }

            dbSchedule.Subject = schedule.Subject == null ? dbSchedule.Subject : schedule.Subject;
            dbSchedule.StartTime = schedule.StartTime ?? dbSchedule.StartTime;
            dbSchedule.EndTime = schedule.EndTime ?? dbSchedule.EndTime;
            dbSchedule.StartTimezone = schedule.StartTimezone ?? dbSchedule.StartTimezone;
            dbSchedule.EndTimezone = schedule.EndTimezone ?? dbSchedule.EndTimezone;
            dbSchedule.Status = schedule.Status??dbSchedule.Status;
            dbSchedule.Description = schedule.Description ?? dbSchedule.Description;
            dbSchedule.Location = schedule.Location ?? dbSchedule.Location;
            dbSchedule.IsAllDay = schedule.IsAllDay;
            dbContext.Update(dbSchedule);
            await dbContext.SaveChangesAsync();
            return;
        }
    }
}
