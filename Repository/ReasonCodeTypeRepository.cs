using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;


namespace CWTDesktopDatabase.Repository
{
    public class ReasonCodeTypeRepository
    {
        private ReasonCodeTypeDC db = new ReasonCodeTypeDC(Settings.getConnectionString());


        //Get one Item
        public ReasonCodeType GetItem(int reasonCodeTypeId)
        {
            return db.ReasonCodeTypes.SingleOrDefault(c => c.ReasonCodeTypeId == reasonCodeTypeId);
        }

        //Get one Item
        public IQueryable<ReasonCodeType> GetAllReasonCodeTypes()
        {
            return db.ReasonCodeTypes.OrderBy(c => c.ReasonCodeTypeDescription);
        }
    }
}
