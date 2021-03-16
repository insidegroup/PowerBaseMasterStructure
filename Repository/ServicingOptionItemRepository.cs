using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class ServicingOptionItemRepository
    {
        private ServicingOptionItemDC db = new ServicingOptionItemDC(Settings.getConnectionString());

        //Get a Page of ServicingOptionItems - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectServicingOptionItems_v1Result> PageServicingOptionItems(int servicingOptionGroupId, int page, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectServicingOptionItems_v1(servicingOptionGroupId, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectServicingOptionItems_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get one ServicingOptionItem
        public ServicingOptionItem GetItem(int servicingOptionItemId)
        {
            return db.ServicingOptionItems.SingleOrDefault(c => c.ServicingOptionItemId == servicingOptionItemId);
        }

        //Add Data From Linked Tables for Display
        public void EditItemForDisplay(ServicingOptionItem servicingOptionItem)
        {
            ServicingOptionRepository servicingOptionRepository = new ServicingOptionRepository();
            ServicingOption servicingOption = new ServicingOption();
            servicingOption = servicingOptionRepository.GetServicingOption(servicingOptionItem.ServicingOptionId);
            if (servicingOption != null)
            {
                servicingOptionItem.ServicingOptionName = servicingOption.ServicingOptionName;
				servicingOptionItem.GDSRequiredFlag = servicingOption.GDSRequiredFlag;
            }

            ServicingOptionGroupRepository servicingOptionGroupRepository = new ServicingOptionGroupRepository();
            ServicingOptionGroup servicingOptionGroup = new ServicingOptionGroup();
            servicingOptionGroup = servicingOptionGroupRepository.GetGroup(servicingOptionItem.ServicingOptionGroupId);
            if (servicingOptionGroup != null)
            {
                servicingOptionItem.ServicingOptionGroupName = servicingOptionGroup.ServicingOptionGroupName;

            }

            if(servicingOptionItem.GDSCode!=null){
                GDSRepository gDSRepository = new GDSRepository();
                GDS gds = new GDS();
                gds = gDSRepository.GetGDS(servicingOptionItem.GDSCode);
                if (gds != null)
                {
                    servicingOptionItem.GDSName = gds.GDSName;

                }
            }

			//Get ServicingOptionFareCalculations
			ServicingOptionFareCalculation servicingOptionFareCalculation = new ServicingOptionFareCalculation();
			servicingOptionFareCalculation = db.ServicingOptionFareCalculations.SingleOrDefault(c => c.ServicingOptionItemId == servicingOptionItem.ServicingOptionItemId);
			if (servicingOptionFareCalculation != null)
			{
				servicingOptionItem.DepartureTimeWindowMinutes = servicingOptionFareCalculation.DepartureTimeWindowMinutes;
				servicingOptionItem.ArrivalTimeWindowMinutes = servicingOptionFareCalculation.ArrivalTimeWindowMinutes;
				servicingOptionItem.MaximumConnectionTimeMinutes = servicingOptionFareCalculation.MaximumConnectionTimeMinutes; 
				servicingOptionItem.MaximumStops = servicingOptionFareCalculation.MaximumStops;
				servicingOptionItem.UseAlternateAirportFlag = servicingOptionFareCalculation.UseAlternateAirportFlag;
				servicingOptionItem.NoPenaltyFlag = servicingOptionFareCalculation.NoPenaltyFlag;
				servicingOptionItem.NoRestrictionsFlag = servicingOptionFareCalculation.NoRestrictionsFlag;
			}
        }

        //Add ServicingOptionItem 
        public void Add(ServicingOptionItem servicingOptionItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_InsertServicingOptionItem_v1(
                servicingOptionItem.ServicingOptionGroupId,
                servicingOptionItem.ServicingOptionId,
                servicingOptionItem.ServicingOptionItemValue,
                servicingOptionItem.GDSCode,
                servicingOptionItem.ServicingOptionItemInstruction,
                servicingOptionItem.DisplayInApplicationFlag,
				servicingOptionItem.DepartureTimeWindowMinutes,
				servicingOptionItem.ArrivalTimeWindowMinutes,
				servicingOptionItem.MaximumStops,
				servicingOptionItem.MaximumConnectionTimeMinutes,
				servicingOptionItem.UseAlternateAirportFlag,
				servicingOptionItem.NoPenaltyFlag,
				servicingOptionItem.NoRestrictionsFlag,
                adminUserGuid
            );
        }

        //Edit ServicingOptionItem 
        public void Edit(ServicingOptionItem servicingOptionItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_UpdateServicingOptionItem_v1(
                servicingOptionItem.ServicingOptionItemId,
                servicingOptionItem.ServicingOptionId,
                servicingOptionItem.ServicingOptionItemValue,
                servicingOptionItem.GDSCode,
                servicingOptionItem.ServicingOptionItemInstruction,
                servicingOptionItem.DisplayInApplicationFlag,
				servicingOptionItem.DepartureTimeWindowMinutes,
				servicingOptionItem.ArrivalTimeWindowMinutes,
				servicingOptionItem.MaximumStops,
				servicingOptionItem.MaximumConnectionTimeMinutes,
				servicingOptionItem.UseAlternateAirportFlag,
				servicingOptionItem.NoPenaltyFlag,
				servicingOptionItem.NoRestrictionsFlag,
				adminUserGuid,
                servicingOptionItem.VersionNumber
            );
        }

        //Delete ServicingOptionItem
        public void Delete(ServicingOptionItem servicingOptionItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

            db.spDesktopDataAdmin_DeleteServicingOptionItem_v1(
                servicingOptionItem.ServicingOptionItemId,
                adminUserGuid,
                servicingOptionItem.VersionNumber);
        }

        //REMOVED: List of All Items - Sortable
        /*public IQueryable<fnDesktopDataAdmin_SelectServicingOptionItems_v1Result> GetServicingOptionItems(int servicingOptionGroupId, string sortField, int sortOrder)
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
            var result = db.fnDesktopDataAdmin_SelectServicingOptionItems_v1(servicingOptionGroupId, adminUserGuid).OrderBy(sortField);
            return result;
        }*/

    }
}
