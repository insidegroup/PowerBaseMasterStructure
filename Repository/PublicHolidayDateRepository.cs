using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class PublicHolidayDateRepository
    {
        private PublicHolidayDateDC db = new PublicHolidayDateDC(Settings.getConnectionString());

        //Get a Page of PublicHolidayDates - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectPublicHolidayDates_v1Result> PagePublicHolidayDates(int publicHolidayGroupId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectPublicHolidayDates_v1(publicHolidayGroupId, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPublicHolidayDates_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one PublicHolidayGroupHolidayDate
        public PublicHolidayGroupHolidayDate GetItem(int publicHolidayDateId)
        {
            return db.PublicHolidayGroupHolidayDates.SingleOrDefault(c => c.PublicHolidayDateId == publicHolidayDateId);
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(PublicHolidayGroupHolidayDate publicHolidayGroupHolidayDate)
        {

            PublicHolidayGroupRepository publicHolidayGroupRepository = new PublicHolidayGroupRepository();
            PublicHolidayGroup publicHolidayGroup = new PublicHolidayGroup();
            publicHolidayGroup = publicHolidayGroupRepository.GetGroup(publicHolidayGroupHolidayDate.PublicHolidayGroupId);
            if (publicHolidayGroup != null)
            {
                publicHolidayGroupHolidayDate.PublicHolidayGroupName = publicHolidayGroup.PublicHolidayGroupName;
            }
        }

        //Add Item 
        public void Add(PublicHolidayGroupHolidayDate publicHolidayGroupHolidayDate)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertPublicHolidayDate_v1(
                publicHolidayGroupHolidayDate.PublicHolidayDate1,
                publicHolidayGroupHolidayDate.PublicHolidayDescription,
                publicHolidayGroupHolidayDate.PublicHolidayGroupId,
                adminUserGuid
            );
        }

        //Edit Item 
        public void Edit(PublicHolidayGroupHolidayDate publicHolidayGroupHolidayDate)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdatePublicHolidayDate_v1(
                publicHolidayGroupHolidayDate.PublicHolidayDateId,
                publicHolidayGroupHolidayDate.PublicHolidayDate1,
                publicHolidayGroupHolidayDate.PublicHolidayDescription,
                adminUserGuid,
                publicHolidayGroupHolidayDate.VersionNumber
            );
        }

        //Delete Item
        public void Delete(PublicHolidayGroupHolidayDate publicHolidayGroupHolidayDate)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeletePublicHolidayDate_v1(
                publicHolidayGroupHolidayDate.PublicHolidayDateId,
                adminUserGuid,
                publicHolidayGroupHolidayDate.VersionNumber);
        }

        //REMOVED: List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectPublicHolidayDates_v1Result> GetPublicHolidayDates(int publicHolidayDateId, string sortField, int sortOrder)
        {
            if (sortOrder == 0)
            {
                sortField = sortField + " ascending";
            }
            else
            {
                sortField = sortField + " descending";
            }

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.fnDesktopDataAdmin_SelectPublicHolidayDates_v1(publicHolidayDateId, adminUserGuid).OrderBy(sortField);
            return result;
        }*/


    }
}
