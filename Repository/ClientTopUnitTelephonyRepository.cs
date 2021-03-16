using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientTopUnitTelephonyRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ClientTopUnitTelephony Items - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitTelephony_v1Result> PageClientTopUnitTelephonies(int page, string id, string sortField, int? sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectClientTopUnitTelephony_v1(id, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitTelephony_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

    }
}