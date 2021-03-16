using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
	public class PriceTrackingEmailAlertTypeRepository
	{
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		public PriceTrackingEmailAlertType GetPriceTrackingEmailAlertType(int id)
		{
			return db.PriceTrackingEmailAlertTypes.Where(c => c.PriceTrackingEmailAlertTypeId == id).SingleOrDefault();
		}

        public IQueryable<PriceTrackingEmailAlertType> GetAllPriceTrackingEmailAlertTypes()
		{
			return db.PriceTrackingEmailAlertTypes.OrderBy(c => c.PriceTrackingEmailAlertTypeName);
		}
	}
}