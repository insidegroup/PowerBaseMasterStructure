using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
namespace CWTDesktopDatabase.Repository
{
    public class ClientDetailTravelerTypeRepository
    {
        private ClientDetailDC db = new ClientDetailDC(Settings.getConnectionString());

        //Get a Page of ClientDetails
        public CWTPaginatedList<spDesktopDataAdmin_SelectTravelerTypeClientDetails_v1Result> GetClientDetailsByDeletedFlag(string travelerTypeGuid, string filter, bool deleted, string sortField, int sortOrder, int page)
        {
            var result = db.spDesktopDataAdmin_SelectTravelerTypeClientDetails_v1(travelerTypeGuid, deleted, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTravelerTypeClientDetails_v1Result>(result, page, Convert.ToInt32(totalRecords));
            return paginatedView;
        }

        public ClientDetailTravelerType GetClientDetailTravelerType(int clientDetailId)
        {
            return db.ClientDetailTravelerTypes.SingleOrDefault(c => c.ClientDetailId == clientDetailId);
        }

        //Add Group
        public void Add(TravelerTypeClientDetailVM travelerTypeClientDetailVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertClientDetail_v1(
                travelerTypeClientDetailVM.ClientDetail.WebsiteAddress,
                travelerTypeClientDetailVM.ClientDetail.Logo,
                travelerTypeClientDetailVM.ClientDetail.InheritFromParentFlag,
                travelerTypeClientDetailVM.ClientDetail.ClientDetailName,
                travelerTypeClientDetailVM.ClientDetail.EnabledFlag,
                travelerTypeClientDetailVM.ClientDetail.EnabledDate,
                travelerTypeClientDetailVM.ClientDetail.ExpiryDate,
                travelerTypeClientDetailVM.ClientDetail.TripTypeId,
                "TravelerType",
                travelerTypeClientDetailVM.TravelerType.TravelerTypeGuid,
                null,
                null,
                null,
                adminUserGuid
            );
        }

        //Edit Group
        public void Edit(TravelerTypeClientDetailVM travelerTypeClientDetailVM)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateClientDetail_v1(

                travelerTypeClientDetailVM.ClientDetail.ClientDetailId,
                travelerTypeClientDetailVM.ClientDetail.WebsiteAddress,
                travelerTypeClientDetailVM.ClientDetail.Logo,
                travelerTypeClientDetailVM.ClientDetail.InheritFromParentFlag,
                travelerTypeClientDetailVM.ClientDetail.ClientDetailName,
                travelerTypeClientDetailVM.ClientDetail.EnabledFlag,
                travelerTypeClientDetailVM.ClientDetail.EnabledDate,
                travelerTypeClientDetailVM.ClientDetail.ExpiryDate,
                travelerTypeClientDetailVM.ClientDetail.TripTypeId,
                "TravelerType",
                travelerTypeClientDetailVM.TravelerType.TravelerTypeGuid,
                null,
                null,
                null,
                adminUserGuid,
                travelerTypeClientDetailVM.ClientDetail.VersionNumber
            );
        }
    }
}