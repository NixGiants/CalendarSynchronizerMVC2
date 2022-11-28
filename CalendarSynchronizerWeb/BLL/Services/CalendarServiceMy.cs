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
    public class CalendarServiceMy : ICalendarService
    {
        private readonly ICalendarRepository calendarRepository;
        private readonly ILogger<CalendarServiceMy> logger;

        public CalendarServiceMy(ICalendarRepository calendarRepository, ILogger<CalendarServiceMy> logger)
        {
            this.calendarRepository = calendarRepository;
            this.logger = logger;
        }

        public async Task Create(CalendarMy calendar)
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

        public async Task<CalendarMy?> Get(string calendarId)
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

        public async Task<List<CalendarMy>> GetAll(string sortOrder, string? searchString)
        {
            List<CalendarMy> calendars = new List<CalendarMy>();

            try
            {
                calendars = await calendarRepository.GetAll();
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            switch (sortOrder)
            {
                case "summary_desc":
                    calendars = calendars.OrderByDescending(s => s.Summary).ToList();
                    break;
                case "description_desc":
                    calendars = calendars.OrderByDescending(c => c.Description).ToList();
                    break;
                case "Description":
                    calendars = calendars.OrderBy(c => c.Description).ToList();
                    break;
                case "CalendarId_desc":
                    calendars = calendars.OrderByDescending(c => c.CalendarId).ToList();
                    break;
                case "CalendarId":
                    calendars = calendars.OrderBy(c => c.CalendarId).ToList();
                    break;
                default:
                    calendars = calendars.OrderBy(c => c.Summary).ToList();
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                calendars = calendars.Where(c => c.Description.Contains(searchString)).ToList();
            }

            return calendars;
        }

        public async Task<List<CalendarMy>> GetByUserId(string userId, string? searchString = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("Bad Id was given");
            }

            List<CalendarMy> calendars = new List<CalendarMy>();

            try
            {
                calendars = await calendarRepository.GetByUserId(userId);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                calendars = calendars.Where(c => c.Description.Contains(searchString)).ToList();
            }

            return calendars;
        }

        public async Task Update(CalendarMy calendar, string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Bad Id was given");
            }
            try
            {
                await calendarRepository.Update(calendar, id);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return;
        }
    }
}
