

namespace Core.Models
{
    public class ScheduleComparer : IComparer<Schedule>
    {
        public int Compare(Schedule x, Schedule y)
        {

            if (x.Status.CompareTo(y.Status) != 0)
            {
                return x.Status.CompareTo(y.Status);
            }
            else if (x.ExternalId.CompareTo(y.ExternalId) != 0)
            {
                return x.ExternalId.CompareTo(y.ExternalId);
            }
            else
            {
                return 0;
            }
        }
    }
}
