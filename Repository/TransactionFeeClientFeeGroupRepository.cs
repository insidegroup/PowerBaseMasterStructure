using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class TransactionFeeClientFeeGroupRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of TransactionFees - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupTransactionFees_v1Result> PageTransactionFeeClientFeeGroups(int clientFeeGroupId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectClientFeeGroupTransactionFees_v1(clientFeeGroupId, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientFeeGroupTransactionFees_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //TransactionFees not used by this ClientFeeGroup
        public List<TransactionFee> GetUnUsedTransactionFees(int clientFeeGroupId, int productId, int? transactionFeeId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectClientFeeGroupAvailableTransactionFees_v1(clientFeeGroupId, productId, transactionFeeId)
                         select new TransactionFee
                         {
                             TransactionFeeId = n.TransactionFeeId,
                             TransactionFeeDescription = n.TransactionFeeDescription
                         };
			return result.OrderBy(x => x.TransactionFeeDescription).ToList();
        }

        //Get a single MerchantFeeClientFeeGroup 
        public TransactionFeeClientFeeGroup GetItem(int clientFeeGroupId, int transactionFeeId)
        {
            return db.TransactionFeeClientFeeGroups.SingleOrDefault(c =>
                (c.ClientFeeGroupId == clientFeeGroupId && c.TransactionFeeId == transactionFeeId)
                );
        }

        //Add TransactionFeeClientFeeGroup
        public void Add(TransactionFeeClientFeeGroup transactionFeeClientFeeGroup)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertTransactionFeeClientFeeGroup_v1(
                transactionFeeClientFeeGroup.ClientFeeGroupId,
                transactionFeeClientFeeGroup.TransactionFeeId,
                adminUserGuid
            );
        }

        //Update MerchantFeeClientFeeGroup
        public void Update(TransactionFeeClientFeeGroup transactionFeeClientFeeGroup, int originalTransactionFeeId)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateTransactionFeeClientFeeGroup_v1(
                transactionFeeClientFeeGroup.ClientFeeGroupId,
                transactionFeeClientFeeGroup.TransactionFeeId,
                originalTransactionFeeId,
                adminUserGuid,
                transactionFeeClientFeeGroup.VersionNumber
            );
        }

        //Delete From DB
        public void Delete(TransactionFeeClientFeeGroup transactionFeeClientFeeGroup)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteTransactionFeeClientFeeGroup_v1(
                transactionFeeClientFeeGroup.ClientFeeGroupId,
                transactionFeeClientFeeGroup.TransactionFeeId,
                transactionFeeClientFeeGroup.VersionNumber,
                adminUserGuid
            );
        }
    }
}