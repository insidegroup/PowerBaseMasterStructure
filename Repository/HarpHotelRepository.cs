using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class HarpHotelRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<HarpHotel> GetAllHarpHotels()
        {
            return db.HarpHotels.OrderBy(c => c.HarpHotelName);
        }

        public HarpHotel GetHarpHotel(int harpHotelId)
        {
            return db.HarpHotels.SingleOrDefault(c => c.HarpHotelId == harpHotelId);
        }
    }
}

