using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class TripTypeRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get All TripTypes
        public IQueryable<TripType> GetAllTripTypes()
        {
            return db.TripTypes.OrderBy(c => c.TripTypeDescription);
        }

        //Get a Page of TripTypes - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectTripTypes_v1Result> PageTripTypes(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectTripTypes_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectTripTypes_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Single TripType
        public TripType GetTripType(int? tripTypeId)
        {
            return db.TripTypes.SingleOrDefault(c => c.TripTypeId == tripTypeId);
        }

        //Add a TripType
        public void Add(TripType tripType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertTripType_v1(
                tripType.TripTypeDescription,
                tripType.BackOfficeTripTypeCode,
                adminUserGuid
            );
        }

        //Edit a TripType
        public void Update(TripType tripType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateTripType_v1(
                tripType.TripTypeId,
                tripType.TripTypeDescription,
                tripType.BackOfficeTripTypeCode,
                adminUserGuid,
                tripType.VersionNumber
            );
        }

        //Delete a TripType
        public void Delete(TripType tripType)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteTripType_v1(
               tripType.TripTypeId,
               adminUserGuid,
               tripType.VersionNumber
           );
        }




        
    }
}
