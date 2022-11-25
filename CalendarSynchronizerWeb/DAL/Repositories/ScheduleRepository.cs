using Core.Models;
using DAL.Interfaces;

namespace DAL.Repositories
{
    internal class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext dbContext;

        public ScheduleRepository(ApplicationDbContext context)
        {
            dbContext = context;
        }

        public Task Create(Schedule schedule)
        {
            throw new NotImplementedException();
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Schedule GetSchedule(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Schedule>> GetUserSchedules(string userId)
        {
            throw new NotImplementedException();
        }

        public Task Update(Guid id, Schedule schedule)
        {
            throw new NotImplementedException();
        }

        Task<Schedule> IScheduleRepository.GetSchedule(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
