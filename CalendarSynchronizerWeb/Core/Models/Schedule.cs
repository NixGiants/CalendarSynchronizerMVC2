using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Schedule
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
    }
}
