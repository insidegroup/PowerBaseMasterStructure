using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientDetailClientAccountRepository
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());

        //Get one Item
        //public ClientDetailClientAccount GetClientDetailsByDeletedFlag(string can, string ssc)
        //{
        //    return db.ClientDetailClientAccounts.SingleOrDefault(c => (c.ClientAccountNumber.Equals(can) && c.SourceSystemCode.Equals(ssc)));
        //}

        //Get a Page of ClientDetails
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientAccountClientDetails_v1Result> GetClientDetailsByDeletedFlag(string clientAccountNumber, string sourceSystemCode, string filter, bool deleted, string sortField, int sortOrder, int page)
        {
            var result = db.spDesktopDataAdmin_SelectClientAccountClientDetails_v1(sourceSystemCode, clientAccountNumber, deleted, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientAccountClientDetails_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        public ClientDetailClientAccount GetClientDetailClientAccount(int clientDetailId)
        {
            return db.ClientDetailClientAccounts.SingleOrDefault(c => c.ClientDetailId == clientDetailId);
        }

        //Add Group
        public void Add(ClientAccountClientDetailVM clientAccountClientDetailVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientDetail_v1(
                clientAccountClientDetailVM.ClientDetail.WebsiteAddress,
                clientAccountClientDetailVM.ClientDetail.Logo,
                clientAccountClientDetailVM.ClientDetail.InheritFromParentFlag,
                clientAccountClientDetailVM.ClientDetail.ClientDetailName,
                clientAccountClientDetailVM.ClientDetail.EnabledFlag,
                clientAccountClientDetailVM.ClientDetail.EnabledDate,
                clientAccountClientDetailVM.ClientDetail.ExpiryDate,
                clientAccountClientDetailVM.ClientDetail.TripTypeId,
                "ClientAccount",
                clientAccountClientDetailVM.ClientAccount.ClientAccountNumber,
                null,
                null,
                clientAccountClientDetailVM.ClientAccount.SourceSystemCode,
                adminUserGuid
            );
        }
     
        //Edit Group
        public void Edit(ClientAccountClientDetailVM clientAccountClientDetailVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientDetail_v1(

                clientAccountClientDetailVM.ClientDetail.ClientDetailId,
                clientAccountClientDetailVM.ClientDetail.WebsiteAddress,
                clientAccountClientDetailVM.ClientDetail.Logo,
                clientAccountClientDetailVM.ClientDetail.InheritFromParentFlag,
                clientAccountClientDetailVM.ClientDetail.ClientDetailName,
                clientAccountClientDetailVM.ClientDetail.EnabledFlag,
                clientAccountClientDetailVM.ClientDetail.EnabledDate,
                clientAccountClientDetailVM.ClientDetail.ExpiryDate,
                clientAccountClientDetailVM.ClientDetail.TripTypeId,
                "ClientAccount",
                clientAccountClientDetailVM.ClientAccount.ClientAccountNumber,
                null,
                null,
                clientAccountClientDetailVM.ClientAccount.SourceSystemCode,
                adminUserGuid,
                clientAccountClientDetailVM.ClientDetail.VersionNumber
            );
        }
    }
}