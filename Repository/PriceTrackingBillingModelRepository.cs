using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
	public class PriceTrackingBillingModelRepository
	{
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		public PriceTrackingBillingModel GetPriceTrackingBillingModel(int id)
		{
			return db.PriceTrackingBillingModels.Where(c => c.PriceTrackingBillingModelId == id).SingleOrDefault();
		}

        public IQueryable<PriceTrackingBillingModel> GetAllPriceTrackingBillingModels()
		{
			return db.PriceTrackingBillingModels.OrderBy(c => c.PriceTrackingBillingModelName);
		}
	}
}