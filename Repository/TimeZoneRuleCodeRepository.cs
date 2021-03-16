using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class TimeZoneRuleRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<TimeZoneRule> GetAllTimeZoneRules()
        {
            return db.TimeZoneRules.OrderBy(t => t.TimeZoneRuleCodeDesc);
        }

        public TimeZoneRule GetTimeZoneRule(string timeZoneRuleCode)
        {
            return db.TimeZoneRules.SingleOrDefault(c => c.TimeZoneRuleCode == timeZoneRuleCode);
        }
    }
}

