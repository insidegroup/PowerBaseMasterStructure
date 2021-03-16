using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class TripTypeItemRepository
    {
        private TripTypeItemDC db = new TripTypeItemDC(Settings.getConnectionString());

        //Get a Page of ControlValues - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectTripTypeItems_v1Result> PageTripTypeItems(int tripTypeGroupId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectTripTypeItems_v1(tripTypeGroupId, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTripTypeItems_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one Item
        public TripTypeItem GetItem(int tripTypeGroupId, int tripTypeId)
        {
            return db.TripTypeItems.SingleOrDefault(c => (c.TripTypeGroupId == tripTypeGroupId)
                    && (c.TripTypeId == tripTypeId));
        }

        //TripTypes not used by this Group
        public List<TripType> GetUnUsedTripTypes(int tripTypeGroupId)
        {

            var result = from n in db.spDesktopDataAdmin_SelectTripTypeGroupAvailableTripTypeItems_v1(tripTypeGroupId)
                         select new TripType
                         {
                             TripTypeId = n.TripTypeId,
                             TripTypeDescription = n.TripTypeDescription
                         };
            return result.ToList();
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(TripTypeItem tripTypeItem)
        {
            //Add Descrription
            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            TripType tripType = new TripType();
            tripType = tripTypeRepository.GetTripType(tripTypeItem.TripTypeId);
            if (tripType != null)
            {
                tripTypeItem.TripTypeDescription = tripType.TripTypeDescription;
            }

            //Add PolicyGroupName
            TripTypeGroupRepository tripTypeGroupRepository = new TripTypeGroupRepository();
            TripTypeGroup tripTypeGroup = new TripTypeGroup();
            tripTypeGroup = tripTypeGroupRepository.GetGroup(tripTypeItem.TripTypeGroupId);
            if (tripTypeGroup != null)
            {
                tripTypeItem.TripTypeGroupName = tripTypeGroup.TripTypeGroupName;
            }

        }

        //Add
        public void Add(TripTypeItem tripTypeItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertTripTypeItem_v1(
                tripTypeItem.TripTypeGroupId,
                tripTypeItem.TripTypeId,
                tripTypeItem.DefaultTripTypeFlag,
                adminUserGuid
            );
        }

        //Edit
        public void Edit(TripTypeItem tripTypeItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateTripTypeItem_v1(
                tripTypeItem.TripTypeGroupId,
                tripTypeItem.TripTypeId,
                tripTypeItem.DefaultTripTypeFlag,
                adminUserGuid,
                tripTypeItem.VersionNumber
            );
        }

        //Delete
        public void Delete(TripTypeItem tripTypeItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteTripTypeItem_v1(
                tripTypeItem.TripTypeGroupId,
                tripTypeItem.TripTypeId,
                adminUserGuid,
                tripTypeItem.VersionNumber
            );
        }

    }
}
