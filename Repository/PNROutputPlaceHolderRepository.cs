using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;

namespace CWTDesktopDatabase.Repository
{
    public class PNROutputPlaceHolderRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        public IQueryable<PNROutputPlaceHolder> GetAllPNROutputPlaceHolders()
        {
            return db.PNROutputPlaceHolders.OrderBy(c => c.PNROutputPlaceHolderName);
        }

        public PNROutputPlaceHolder GetPNROutputPlaceHolder(string pnrOutputPlaceHolderName)
        {
            return db.PNROutputPlaceHolders.SingleOrDefault(c => c.PNROutputPlaceHolderName == pnrOutputPlaceHolderName);
        }
    }
}
