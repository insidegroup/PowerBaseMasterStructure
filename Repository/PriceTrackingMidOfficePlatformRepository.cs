using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
	public class PriceTrackingMidOfficePlatformRepository
	{
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		public PriceTrackingMidOfficePlatform GetPriceTrackingMidOfficePlatform(int id)
		{
			return db.PriceTrackingMidOfficePlatforms.Where(c => c.PriceTrackingMidOfficePlatformId == id).SingleOrDefault();
		}

        public IQueryable<PriceTrackingMidOfficePlatform> GetAllPriceTrackingMidOfficePlatforms()
		{
			return db.PriceTrackingMidOfficePlatforms.OrderBy(c => c.PriceTrackingMidOfficePlatformName);
		}
	}
}