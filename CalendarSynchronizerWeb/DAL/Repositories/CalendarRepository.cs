using Core.Models;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CalendarRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Create(Calendar calendar)
        {
            dbContext.Calendars.Add(calendar);
            await dbContext.SaveChangesAsync();
            return;
        }

        public async Task Delete(string id)
        {
            var calendar = await dbContext.Calendars.FindAsync(id);

            if (calendar == null)
            {
                throw new Exception($"No Calendar With {id} exception");
            }

            dbContext.Calendars.Remove(calendar);
            await dbContext.SaveChangesAsync();
            return;
        }

        public async Task<List<Calendar>> GetAll()
        {
            return await dbContext.Calendars.Include(c => c.AppUser).ToListAsync();
        }

        public async Task<List<Calendar>> GetByUserId(string userId)
        {
            return await dbContext.Calendars.Where(c => c.AppUserId == userId).Include(c => c.AppUser).ToListAsync();
        }

        public async Task Update(Calendar calendar, string id)
        {
            var dbCalendar = dbContext.Calendars.Find(id);

            if (dbCalendar == null)
            {
                throw new Exception($"No Calendar with id {id} was found to update");
            }

            dbCalendar.Description = string.IsNullOrEmpty(calendar.Description) ? dbCalendar.Description : calendar.Description;
            dbCalendar.Summary = string.IsNullOrEmpty(calendar.Summary) ? dbCalendar.Summary : calendar.Summary;
            dbCalendar.TimeZone = string.IsNullOrEmpty(calendar.TimeZone) ? dbCalendar.TimeZone : calendar.TimeZone;
            dbContext.Calendars.Update(dbCalendar);
            await dbContext.SaveChangesAsync();
            return;
        }
    }
}
