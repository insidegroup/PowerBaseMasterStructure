using CWTDesktopDatabase.Models;
using System.Linq;

namespace CWTDesktopDatabase.Repository
{
    public class TravelIndicatorRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());


        public IQueryable<TravelIndicator> GetAllTravelIndicators()
        {
            return db.TravelIndicators.OrderBy(c => c.TravelIndicator1);
        }

        public TravelIndicator GetTravelIndicator(string travelIndicator)
        {
            return db.TravelIndicators.SingleOrDefault(c => c.TravelIndicator1 == travelIndicator);
        }
    }
}
