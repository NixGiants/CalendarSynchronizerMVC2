

namespace Core.Models
{
    public enum State
    {
        Create = 1,
        Update,
        Delete
    }
    public class ScheduleStateChanged
    {
        public Schedule Source { get; set; }
        public Schedule Target { get; set; }
        public State Action { get; set; }
    }
}
