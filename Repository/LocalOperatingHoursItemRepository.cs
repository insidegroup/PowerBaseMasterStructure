using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;
using System.Text;

namespace CWTDesktopDatabase.Repository
{
    public class LocalOperatingHoursItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of LocalOperatingHoursItems - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectLocalOperatingHoursItems_v1Result> PageLocalOperatingHoursItems(int localOperatingHoursItemId, int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectLocalOperatingHoursItems_v1(localOperatingHoursItemId, filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectLocalOperatingHoursItems_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one LocalOperatingHoursItem
		public LocalOperatingHoursItem GetItem(int localOperatingHoursGroupId, int weekDayId, DateTime serviceFundStartTime)
		{
			return db.LocalOperatingHoursItems.SingleOrDefault(c =>
					(c.LocalOperatingHoursGroupId == localOperatingHoursGroupId) &&
					(c.WeekDayId == weekDayId) &&
					(c.OpeningDateTime == serviceFundStartTime)
					);
		}

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(LocalOperatingHoursItem localOperatingHoursItem)
        {
            WeekdayRepository weekdayRepository = new WeekdayRepository();
            Weekday weekday = new Weekday();
            weekday = weekdayRepository.GetWeekday(localOperatingHoursItem.WeekDayId);
            if (weekday != null)
            {
                localOperatingHoursItem.WeekdayName = weekday.WeekdayName;
            }

            LocalOperatingHoursGroupRepository localOperatingHoursGroupRepository = new LocalOperatingHoursGroupRepository();
            LocalOperatingHoursGroup localOperatingHoursGroup = new LocalOperatingHoursGroup();
            localOperatingHoursGroup = localOperatingHoursGroupRepository.GetGroup(localOperatingHoursItem.LocalOperatingHoursGroupId);
			if (localOperatingHoursGroup != null)
			{
				localOperatingHoursItem.LocalOperatingHoursGroupName = localOperatingHoursGroup.LocalOperatingHoursGroupName;
			}
            
            string openingTime;
            
			if (localOperatingHoursItem.OpeningDateTime.Hour < 10)
            {
                openingTime = String.Concat("0", localOperatingHoursItem.OpeningDateTime.Hour,":");
            }
            else
            {
                openingTime = String.Concat(localOperatingHoursItem.OpeningDateTime.Hour,":");
            }

			if (localOperatingHoursItem.OpeningDateTime.Minute < 10)
			{
				openingTime = String.Concat(openingTime, "0", localOperatingHoursItem.OpeningDateTime.Minute);
			}
			else
			{
				openingTime = String.Concat(openingTime, localOperatingHoursItem.OpeningDateTime.Minute);
			}

            if (localOperatingHoursItem.OpeningDateTime.Second != 0)
            {
                if (localOperatingHoursItem.OpeningDateTime.Second < 10)
                {
                    openingTime = String.Concat(openingTime, ":0", localOperatingHoursItem.OpeningDateTime.Second);
                }
                else
                {
                    openingTime = String.Concat(openingTime, ":", localOperatingHoursItem.OpeningDateTime.Second);
                }
            }

			localOperatingHoursItem.OpeningTime = openingTime;

            string serviceFundEndTime;
			
			if (localOperatingHoursItem.ClosingDateTime.Hour < 10)
			{
				serviceFundEndTime = String.Concat("0", localOperatingHoursItem.ClosingDateTime.Hour, ":");
			}
			else
			{
				serviceFundEndTime = String.Concat(localOperatingHoursItem.ClosingDateTime.Hour, ":");
			}

			if (localOperatingHoursItem.ClosingDateTime.Minute < 10)
			{
				serviceFundEndTime = String.Concat(serviceFundEndTime, "0", localOperatingHoursItem.ClosingDateTime.Minute);
			}
			else
			{
				serviceFundEndTime = String.Concat(serviceFundEndTime, localOperatingHoursItem.ClosingDateTime.Minute);
			}

            if (localOperatingHoursItem.ClosingDateTime.Second != 0)
            {
                if (localOperatingHoursItem.ClosingDateTime.Second < 10)
                {
                    serviceFundEndTime = String.Concat(serviceFundEndTime, ":0", localOperatingHoursItem.ClosingDateTime.Second);
                }
                else
                {
                    serviceFundEndTime = String.Concat(serviceFundEndTime,  ":", localOperatingHoursItem.ClosingDateTime.Second);
                }
            }
            
            localOperatingHoursItem.ClosingTime = serviceFundEndTime;
        }

        //Add to DB
        public void Add(LocalOperatingHoursItem item)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

          db.spDesktopDataAdmin_InsertLocalOperatingHoursItem_v1(

                item.LocalOperatingHoursGroupId,
                item.WeekDayId,
                item.OpeningDateTime,
                item.ClosingDateTime,
                adminUserGuid
            );
        }

        //Delete From DB
        public void Delete(LocalOperatingHoursItem item)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteLocalOperatingHoursItem_v1(

                item.LocalOperatingHoursGroupId,
                item.WeekDayId,
                item.OpeningDateTime,
                adminUserGuid,
                item.VersionNumber
              );
        }
    }
}
