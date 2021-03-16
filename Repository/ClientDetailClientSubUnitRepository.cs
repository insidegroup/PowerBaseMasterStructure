using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientDetailClientSubUnitRepository    
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());

        //Get a Page of ClientDetails
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitClientDetails_v1Result> GetClientDetailsByDeletedFlag(string clientSubUnitGuid, string filter, bool deleted, string sortField, int sortOrder, int page)
        {
            var result = db.spDesktopDataAdmin_SelectClientSubUnitClientDetails_v1(clientSubUnitGuid, deleted, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitClientDetails_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        public ClientDetailClientSubUnit GetClientDetailClientSubUnit(int clientDetailId)
        {
            return db.ClientDetailClientSubUnits.SingleOrDefault(c => c.ClientDetailId == clientDetailId);
        }

        //Add Group
        public void Add(ClientSubUnitClientDetailVM clientSubUnitClientDetailVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientDetail_v1(
                clientSubUnitClientDetailVM.ClientDetail.WebsiteAddress,
                clientSubUnitClientDetailVM.ClientDetail.Logo,
                clientSubUnitClientDetailVM.ClientDetail.InheritFromParentFlag,
                clientSubUnitClientDetailVM.ClientDetail.ClientDetailName,
                clientSubUnitClientDetailVM.ClientDetail.EnabledFlag,
                clientSubUnitClientDetailVM.ClientDetail.EnabledDate,
                clientSubUnitClientDetailVM.ClientDetail.ExpiryDate,
                clientSubUnitClientDetailVM.ClientDetail.TripTypeId,
                "ClientSubUnit",
                clientSubUnitClientDetailVM.ClientSubUnit.ClientSubUnitGuid,
                null,
                null,
                null,
                adminUserGuid
            );
        }
     
        //Edit Group
        public void Edit(ClientSubUnitClientDetailVM clientSubUnitClientDetailVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientDetail_v1(

                clientSubUnitClientDetailVM.ClientDetail.ClientDetailId,
                clientSubUnitClientDetailVM.ClientDetail.WebsiteAddress,
                clientSubUnitClientDetailVM.ClientDetail.Logo,
                clientSubUnitClientDetailVM.ClientDetail.InheritFromParentFlag,
                clientSubUnitClientDetailVM.ClientDetail.ClientDetailName,
                clientSubUnitClientDetailVM.ClientDetail.EnabledFlag,
                clientSubUnitClientDetailVM.ClientDetail.EnabledDate,
                clientSubUnitClientDetailVM.ClientDetail.ExpiryDate,
                clientSubUnitClientDetailVM.ClientDetail.TripTypeId,
                "ClientSubUnit",
                clientSubUnitClientDetailVM.ClientSubUnit.ClientSubUnitGuid,
                null,
                null,
                null,
                adminUserGuid,
                clientSubUnitClientDetailVM.ClientDetail.VersionNumber
            );
        }
    }
}