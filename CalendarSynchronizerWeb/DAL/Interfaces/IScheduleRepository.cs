using Core.Models;

namespace DAL.Interfaces
{
    internal interface IScheduleRepository
    {
        Schedule GetSchedule(Guid id);
    }
}
