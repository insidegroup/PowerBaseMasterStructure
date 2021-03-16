using CWTDesktopDatabase.Models;
using System.Linq;

namespace CWTDesktopDatabase.Repository
{
    public class BookingSourceRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        public IQueryable<BookingSource> GetAllBookingSources()
        {
            return db.BookingSources.OrderBy(c => c.BookingSourceCode);
        }

        public BookingSource GetBookingSource(string bookingSourceCode)
        {
            return db.BookingSources.SingleOrDefault(c => c.BookingSourceCode.Equals(bookingSourceCode));
        }
    }
}
