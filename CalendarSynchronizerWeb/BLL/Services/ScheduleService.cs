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
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository scheduleRepository;

        private readonly ILogger<ScheduleService> logger;

        public ScheduleService(ILogger<ScheduleService> logger, IScheduleRepository scheduleRepository)
        {
            this.logger = logger;
            this.scheduleRepository = scheduleRepository;
        }

        public async Task Create(Schedule schedule)
        {
            try
            {
                await scheduleRepository.Create(schedule);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return;
        }

        public async Task Delete(Guid id)
        {
            try
            {
                await scheduleRepository.Delete(id);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return;
        }

        public async Task<List<Schedule>> GetCalendarSchedules(string calendarId, string? searchString)
        {
            if (String.IsNullOrEmpty(calendarId))
            {
                throw new ArgumentException("Bad Id was given");
            }

            List<Schedule> schedules = new List<Schedule>();

            try
            {
                schedules = await scheduleRepository.GetCalendarSchedules(calendarId);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                schedules = schedules.Where(sc => sc.Subject.Contains(searchString)).ToList();
            }

            return schedules;
        }

        public async Task<Schedule?> GetSchedule(Guid id)
        {
            try
            {
                var schedule = await scheduleRepository.GetSchedule(id);
                return schedule;
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return null;
        }

        public async Task Update(Guid id, Schedule schedule)
        {
            try
            {
                await scheduleRepository.Update(id, schedule);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }

            return;
        }
    }
}
