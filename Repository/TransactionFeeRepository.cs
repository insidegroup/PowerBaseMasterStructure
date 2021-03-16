using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class TransactionFeeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of TransactionFees - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectTransactionFees_v1Result> PageTransactionFees(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectTransactionFees_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTransactionFees_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        public TransactionFee GetItem(int transactionFeeId)
        {
            return db.TransactionFees.SingleOrDefault(c => c.TransactionFeeId == transactionFeeId);
        }

    }
}