using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientDetailClientTopUnitRepository
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());

        //Get a Page of ClientDetails
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitClientDetails_v1Result> GetClientDetailsByDeletedFlag(string clientTopUnitGuid, string filter, bool deleted, string sortField, int sortOrder, int page)
        {
            var result = db.spDesktopDataAdmin_SelectClientTopUnitClientDetails_v1(clientTopUnitGuid, deleted, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitClientDetails_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        public ClientDetailClientTopUnit GetClientDetailClientTopUnit(int clientDetailId)
        {
            return db.ClientDetailClientTopUnits.SingleOrDefault(c => c.ClientDetailId == clientDetailId);
        }

        //Add Group
        public void Add(ClientTopUnitClientDetailVM clientTopUnitClientDetailVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientDetail_v1(
                clientTopUnitClientDetailVM.ClientDetail.WebsiteAddress,
                clientTopUnitClientDetailVM.ClientDetail.Logo,
                clientTopUnitClientDetailVM.ClientDetail.InheritFromParentFlag,
                clientTopUnitClientDetailVM.ClientDetail.ClientDetailName,
                clientTopUnitClientDetailVM.ClientDetail.EnabledFlag,
                clientTopUnitClientDetailVM.ClientDetail.EnabledDate,
                clientTopUnitClientDetailVM.ClientDetail.ExpiryDate,
                clientTopUnitClientDetailVM.ClientDetail.TripTypeId,
                "ClientTopUnit",
                clientTopUnitClientDetailVM.ClientTopUnit.ClientTopUnitGuid,
                null,
                null,
                null,
                adminUserGuid
            );
        }

        //Edit Group
        public void Edit(ClientTopUnitClientDetailVM clientTopUnitClientDetailVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientDetail_v1(

                clientTopUnitClientDetailVM.ClientDetail.ClientDetailId,
                clientTopUnitClientDetailVM.ClientDetail.WebsiteAddress,
                clientTopUnitClientDetailVM.ClientDetail.Logo,
                clientTopUnitClientDetailVM.ClientDetail.InheritFromParentFlag,
                clientTopUnitClientDetailVM.ClientDetail.ClientDetailName,
                clientTopUnitClientDetailVM.ClientDetail.EnabledFlag,
                clientTopUnitClientDetailVM.ClientDetail.EnabledDate,
                clientTopUnitClientDetailVM.ClientDetail.ExpiryDate,
                clientTopUnitClientDetailVM.ClientDetail.TripTypeId,
                "ClientTopUnit",
                clientTopUnitClientDetailVM.ClientTopUnit.ClientTopUnitGuid,
                null,
                null,
                null,
                adminUserGuid,
                clientTopUnitClientDetailVM.ClientDetail.VersionNumber
            );
        }
    }
}