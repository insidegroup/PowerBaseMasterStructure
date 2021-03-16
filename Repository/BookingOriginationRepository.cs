using CWTDesktopDatabase.Models;
using System.Linq;

namespace CWTDesktopDatabase.Repository
{
    public class BookingOriginationRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());


        public IQueryable<BookingOrigination> GetAllBookingOriginations()
        {
            return db.BookingOriginations.OrderBy(c => c.BookingOriginationDescription);
        }

        public BookingOrigination GetBookingOrigination(string bookingOriginationCode)
        {
            return db.BookingOriginations.SingleOrDefault(c => c.BookingOriginationCode == bookingOriginationCode);
        }
    }
}
