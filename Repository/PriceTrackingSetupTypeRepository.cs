using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
	public class PriceTrackingSetupTypeRepository
	{
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		public PriceTrackingSetupType GetPriceTrackingSetupType(int id)
		{
			return db.PriceTrackingSetupTypes.Where(c => c.PriceTrackingSetupTypeId == id).SingleOrDefault();
		}

        public IQueryable<PriceTrackingSetupType> GetAllPriceTrackingSetupTypes()
		{
			return db.PriceTrackingSetupTypes.OrderBy(c => c.PriceTrackingSetupTypeName);
		}
	}
}