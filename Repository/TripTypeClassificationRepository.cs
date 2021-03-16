using CWTDesktopDatabase.Models;
using System.Linq;

namespace CWTDesktopDatabase.Repository
{
    public class TripTypeClassificationRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        public IQueryable<TripTypeClassification> GetAllTripTypeClassifications()
        {
            return db.TripTypeClassifications.OrderBy(c => c.TripTypeClassificationDescription);
        }

        public TripTypeClassification GetBookingSource(int tripTypeClassificationId)
        {
            return db.TripTypeClassifications.SingleOrDefault(c => c.TripTypeClassificationId == tripTypeClassificationId);
        }
    }
}
