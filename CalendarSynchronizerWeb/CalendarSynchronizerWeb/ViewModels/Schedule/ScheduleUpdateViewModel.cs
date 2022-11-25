using System.ComponentModel.DataAnnotations;

namespace CalendarSynchronizerWeb.ViewModels.Schedule
{
    public class ScheduleUpdateViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public string Subject { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? StartTime { get; set; }
        public string StartTimeZone { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? EndTime { get; set; }
        public string EndTimeZone { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool IsAllDay { get; set; } = false;
        public string? CalendarId { get; set; }
    }
}
