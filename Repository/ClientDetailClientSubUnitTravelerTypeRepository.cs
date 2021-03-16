using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientDetailClientSubUnitTravelerTypeRepository    
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());


        //Get a Page of ClientDetails
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypeClientDetails_v1Result> GetClientDetailsByDeletedFlag(string clientSubUnitGuid, string travelerTypeGuid, string filter, bool deleted, string sortField, int sortOrder, int page)
        {
            var result = db.spDesktopDataAdmin_SelectClientSubUnitTravelerTypeClientDetails_v1(clientSubUnitGuid, travelerTypeGuid, deleted, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitTravelerTypeClientDetails_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        public ClientDetailClientSubUnitTravelerType GetClientDetailClientSubUnitTravelerType(int clientDetailId)
        {
            return db.ClientDetailClientSubUnitTravelerTypes.SingleOrDefault(c => c.ClientDetailId == clientDetailId);
        }

        //Add Group
        public void Add(ClientSubUnitTravelerTypeClientDetailVM clientSubUnitTravelerTypeClientDetailVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientDetail_v1(
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.WebsiteAddress,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.Logo,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.InheritFromParentFlag,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.ClientDetailName,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.EnabledFlag,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.EnabledDate,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.ExpiryDate,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.TripTypeId,
                "ClientSubUnitTravelerType",
                null,
                clientSubUnitTravelerTypeClientDetailVM.TravelerType.TravelerTypeGuid,
                clientSubUnitTravelerTypeClientDetailVM.ClientSubUnit.ClientSubUnitGuid,
                null,
                adminUserGuid
            );
        }
     
        //Edit Group
        public void Edit(ClientSubUnitTravelerTypeClientDetailVM clientSubUnitTravelerTypeClientDetailVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientDetail_v1(

                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.ClientDetailId,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.WebsiteAddress,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.Logo,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.InheritFromParentFlag,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.ClientDetailName,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.EnabledFlag,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.EnabledDate,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.ExpiryDate,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.TripTypeId,
                "ClientSubUnitTravelerType",
                null,
                clientSubUnitTravelerTypeClientDetailVM.TravelerType.TravelerTypeGuid,
                clientSubUnitTravelerTypeClientDetailVM.ClientSubUnit.ClientSubUnitGuid,
                null,
                adminUserGuid,
                clientSubUnitTravelerTypeClientDetailVM.ClientDetail.VersionNumber
            );
        }
    }
}