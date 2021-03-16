using CWTDesktopDatabase.Models;
using System.Linq;

namespace CWTDesktopDatabase.Repository
{
    public class FeeCategoryRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        public IQueryable<FeeCategory> GetAllFeeCategories()
        {
            return db.FeeCategories.OrderBy(c => c.FeeCategory1);
        }

        //public TravelIndicator GetTravelIndicator(string travelIndicator)
        //{
        //    return db.TravelIndicators.SingleOrDefault(c => c.TravelIndicator1 == travelIndicator);
        //}
    }
}
