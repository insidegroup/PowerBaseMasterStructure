using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
	public class PriceTrackingSystemRuleRepository
	{
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		public PriceTrackingSystemRule GetPriceTrackingSystemRule(int id)
		{
			return db.PriceTrackingSystemRules.Where(c => c.PriceTrackingSystemRuleId == id).SingleOrDefault();
		}

        public IQueryable<PriceTrackingSystemRule> GetAllPriceTrackingSystemRules()
		{
			return db.PriceTrackingSystemRules.OrderBy(c => c.PriceTrackingSystemRuleName);
		}
	}
}