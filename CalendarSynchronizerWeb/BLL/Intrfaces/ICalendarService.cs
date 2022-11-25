using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Intrfaces
{
    public interface ICalendarService
    {
        public Task Create(Calendar calendar);
        public Task<List<Calendar>> GetAll();

        public Task<List<Calendar>> GetByUserId(string userId);

        public Task<Calendar?> Get(string calendarId);

        public Task Delete(string id);

        public Task Update(Calendar calendar, string id);

    }
}
