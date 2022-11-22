using Core.Models;
using DAL.Interfaces;

namespace DAL.Repositories
{
    internal class ScheduleRepository : IScheduleRepository
    {
        private readonly ApplicationDbContext _context;

        public ScheduleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Schedule GetSchedule(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
