using System.Linq;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
	public class PriceTrackingItinerarySolutionRepository
	{
		private PriceTrackingDC db = new PriceTrackingDC(Settings.getConnectionString());

		public PriceTrackingItinerarySolution GetPriceTrackingItinerarySolution(int id)
		{
			return db.PriceTrackingItinerarySolutions.Where(c => c.PriceTrackingItinerarySolutionId == id).SingleOrDefault();
		}

        public IQueryable<PriceTrackingItinerarySolution> GetAllPriceTrackingItinerarySolutions()
		{
			return db.PriceTrackingItinerarySolutions.OrderBy(c => c.PriceTrackingItinerarySolutionName);
		}
	}
}