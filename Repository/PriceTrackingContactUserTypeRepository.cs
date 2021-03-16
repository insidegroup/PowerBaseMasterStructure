using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
	public class PriceTrackingContactUserTypeRepository
	{
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		public PriceTrackingContactUserType GetPriceTrackingContactUserType(int id)
		{
			return db.PriceTrackingContactUserTypes.Where(c => c.PriceTrackingContactUserTypeId == id).SingleOrDefault();
		}

        public IQueryable<PriceTrackingContactUserType> GetAllPriceTrackingContactUserTypes()
		{
			return db.PriceTrackingContactUserTypes.OrderBy(c => c.PriceTrackingContactUserTypeName);
		}
	}
}