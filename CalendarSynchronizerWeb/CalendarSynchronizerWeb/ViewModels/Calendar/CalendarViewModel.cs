﻿using System.ComponentModel.DataAnnotations;

namespace CalendarSynchronizerWeb.ViewModels.Calendar
{
    public class CalendarViewModel
    {
        public string? CalendarId { get; set; }

        [Required]
        public string Summary { get; set; }

        public string? Description { get; set; }

        public string? TimeZone { get; set; }

    }
}
