using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class CalendarMy
    {
        [Key]
        public string CalendarId { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Summary { get; set; }

        public string Description { get; set; }

        public string TimeZone { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public List<Schedule> Schedules { get; set; }
        
    }

}
