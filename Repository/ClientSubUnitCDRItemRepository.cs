using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Text.RegularExpressions;
using System.Xml;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Repository
{
	public class ClientSubUnitCDRItemRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of ClientSubUnitCDR Items - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCDRItems_v1Result> PageClientSubUnitCDRItems(int id, int page, string filter, string sortField, int? sortOrder)
        {
            //get a page of records
			var result = db.spDesktopDataAdmin_SelectClientSubUnitCDRItems_v1(id, filter, sortField, sortOrder, page).ToList();
            
            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSubUnitCDRItems_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

        //Get a Single Item
		public ClientSubUnitClientDefinedReferenceItem GetClientSubUnitCDRItem(int clientSubUnitClientDefinedReferenceItemId)
        {
			return db.ClientSubUnitClientDefinedReferenceItems.SingleOrDefault(c => c.ClientSubUnitClientDefinedReferenceItemId == clientSubUnitClientDefinedReferenceItemId);
        }

		//Get ClientSubUnitClientDefinedReferenceItems
		public List<ClientSubUnitClientDefinedReferenceItem> GetClientSubUnitCDRItems(int clientSubUnitClientDefinedReferenceId)
        {
            return db.ClientSubUnitClientDefinedReferenceItems.Where(c => c.ClientSubUnitClientDefinedReferenceId == clientSubUnitClientDefinedReferenceId).ToList();
        }

        //Add to DB
		public void Add(ClientSubUnitClientDefinedReferenceItem clientSubUnitClientDefinedReferenceItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertClientSubUnitCDRItem_v1(
				clientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId,
				clientSubUnitClientDefinedReferenceItem.RelatedToDisplayName,
				clientSubUnitClientDefinedReferenceItem.RelatedToValue,
                adminUserGuid
            );
        }

        //Update DB
		public void Edit(ClientSubUnitClientDefinedReferenceItem clientSubUnitClientDefinedReferenceItem)
		{
             string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			 db.spDesktopDataAdmin_UpdateClientSubUnitCDRItem_v1(
				clientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceItemId,
				clientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceId,
				clientSubUnitClientDefinedReferenceItem.RelatedToDisplayName,
				clientSubUnitClientDefinedReferenceItem.RelatedToValue,
				adminUserGuid,
				clientSubUnitClientDefinedReferenceItem.VersionNumber
            );
        }

         //Delete From DB
		public void Delete(ClientSubUnitClientDefinedReferenceItem clientSubUnitClientDefinedReferenceItem)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteClientSubUnitCDRItem_v1(
				clientSubUnitClientDefinedReferenceItem.ClientSubUnitClientDefinedReferenceItemId,
                adminUserGuid,
				clientSubUnitClientDefinedReferenceItem.VersionNumber
            );
        }
    }
}   