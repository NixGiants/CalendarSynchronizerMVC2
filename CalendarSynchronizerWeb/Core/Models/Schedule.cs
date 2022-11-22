using Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Schedule : IEntity, IComparable<Schedule>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ExternalId { get; set; }
        public string Subject { get; set; }
        public DateTime? StartTime { get; set; }
        public string StartTimezone { get; set; }
        public DateTime? EndTime { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; } = "";
        public string Status { get; set; }
        public string RecurrenceException { get; set; } = "";
        public string? RecurrenceID { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public bool IsAllDay { get; set; } = false;

        public int CompareTo(Schedule other)
        {

            if (Status.CompareTo(other.Status) != 0)
            {
                return Status.CompareTo(other.Status);
            }
            else if (ExternalId.CompareTo(other.ExternalId) != 0)
            {
                return ExternalId.CompareTo(other.ExternalId);
            }
            else
            {
                return 0;
            }
        }

        public void UpdateEntity(Schedule scheduleFromServer)
        {
            Subject = scheduleFromServer.Subject;
            Description = scheduleFromServer.Description;
            StartTime = scheduleFromServer.StartTime;
            EndTime = scheduleFromServer.EndTime;
            StartTimezone = scheduleFromServer.StartTimezone;
            EndTimezone = scheduleFromServer.EndTimezone;
            ExternalId = scheduleFromServer.ExternalId;
            Location = scheduleFromServer.Location;
            RecurrenceID = scheduleFromServer.RecurrenceID;
            RecurrenceRule = scheduleFromServer.RecurrenceRule;
            RecurrenceException = scheduleFromServer.RecurrenceException;
            IsAllDay = scheduleFromServer.IsAllDay;
        }
    }
}

