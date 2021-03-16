using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class TravelerBackOfficeTypeRepository
    {
        private TravelerBackOfficeTypeDC db = new TravelerBackOfficeTypeDC(Settings.getConnectionString());

        public IQueryable<TravelerBackOfficeType> GetAllTravelerBackOfficeTypes()
        {
            return db.TravelerBackOfficeTypes.OrderBy(c => c.TravelerBackOfficeTypeDescription);
        }

        //Get one Item
        public TravelerBackOfficeType GetTravelerBackOfficeType(string travelerBackOfficeTypeCode)
        {
            return db.TravelerBackOfficeTypes.SingleOrDefault(c => c.TravelerBackOfficeTypeCode == travelerBackOfficeTypeCode);
        }
    }
}