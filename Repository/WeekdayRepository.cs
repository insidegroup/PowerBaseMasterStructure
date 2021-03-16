using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class WeekdayRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public Weekday GetWeekday(int weekdayId)
        {
            return db.Weekdays.SingleOrDefault(c => c.WeekdayId == weekdayId);
        }
        public IQueryable<Weekday> GetAllWeekdays()
        {
            return db.Weekdays.OrderBy(c => c.WeekdayId);
        }
    }
}
