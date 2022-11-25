﻿using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICalendarRepository
    {
        public Task Create(Calendar calendar);
        public Task<List<Calendar>> GetAll();

        public Task<List<Calendar>> GetByUserId(string userId);

        public Task Delete(string id);

        public Task Update(Calendar calendar, string id);
    }
}
