using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AppUser:IdentityUser
    {

        [NotMapped]
        public string? RoleId { get; set; }

        [NotMapped]
        public string? Role { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem>? RoleList { get; set; }

        public List<CalendarMy>? Calendars { get; set; }
    }
}
