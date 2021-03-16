using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientFeeRepository
    {

        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ClientFees - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientFees_v1Result> PageClientFees(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectClientFees_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientFees_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public IQueryable<ClientFee> GetClientFeesByType(int feeTypeId)
        {
            return db.ClientFees.Where(c => (c.FeeTypeId == feeTypeId)).OrderBy(c => c.ClientFeeDescription);
        }

        //Get one Item
        public ClientFee GetItem(int clientFeeId)
        {
            return db.ClientFees.SingleOrDefault(c => (c.ClientFeeId == clientFeeId));
        }

        //Add to DB
        public void Add(ClientFeeVM clientFeeVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientFee_v1(
                clientFeeVM.ClientFee.ClientFeeDescription,
                clientFeeVM.ClientFee.FeeTypeId,
                clientFeeVM.ClientFee.ContextId,
                clientFeeVM.ClientFee.GDSCode,
                clientFeeVM.ClientFeeOutput.OutputFormat,
                clientFeeVM.ClientFeeOutput.OutputDescription,
                clientFeeVM.ClientFeeOutput.OutputPlaceholder,
                adminUserGuid
            );


        }

        //Update in DB
        public void Update(ClientFeeVM clientFeeVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientFee_v1(
                clientFeeVM.ClientFee.ClientFeeId,
                clientFeeVM.ClientFee.ClientFeeDescription,
                clientFeeVM.ClientFee.ContextId,
                clientFeeVM.ClientFee.GDSCode,
                clientFeeVM.ClientFeeOutput.OutputFormat,
                clientFeeVM.ClientFeeOutput.OutputDescription,
                clientFeeVM.ClientFeeOutput.OutputPlaceholder,
                adminUserGuid,
                clientFeeVM.ClientFee.VersionNumber                
            );

        }

        //Delete From DB
        public void Delete(ClientFee clientFee)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteClientFee_v1(
                clientFee.ClientFeeId,
                adminUserGuid,
                clientFee.VersionNumber
            );
        }
    }
}