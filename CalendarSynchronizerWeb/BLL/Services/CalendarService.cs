using BLL.Intrfaces;
using Core.Models;
using DAL.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly ICalendarRepository calendarRepository;
        private readonly ILogger<CalendarService> logger;

        public CalendarService(ICalendarRepository calendarRepository, ILogger<CalendarService> logger)
        {
            this.calendarRepository = calendarRepository;
            this.logger = logger;
        }

        public async Task Create(Calendar calendar)
        {
            try
            {
                await calendarRepository.Create(calendar);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return;
        }

        public async Task Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Bad Id was given");
            }
            try
            {
                await calendarRepository.Delete(id);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return;
        }

        public async Task<Calendar?> Get(string calendarId)
        {
            if (string.IsNullOrEmpty(calendarId))
            {
                throw new ArgumentException("Bad Id was given");
            }

            try
            {
                var calendar = await calendarRepository.Get(calendarId);
                return calendar;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return null;
        }

        public async Task<List<Calendar>> GetAll()
        {
            List<Calendar> calendars = new List<Calendar>();

            try
            {
                calendars = await calendarRepository.GetAll();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return calendars;
        }

        public async Task<List<Calendar>> GetByUserId(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Bad Id was given");
            }

            List<Calendar> calendars = new List<Calendar>();

            try
            {
                calendars = await calendarRepository.GetByUserId(userId);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return calendars;
        }

        public async Task Update(Calendar calendar, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Bad Id was given");
            }
            try
            {
                await calendarRepository.Update(calendar ,id);
            }
            catch(Exception e)
            {
                logger.LogError(e.Message);
            }

            return;
        }
    }
}
