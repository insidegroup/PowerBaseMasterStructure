using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Repository
{
    public class ClientTelephonyRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

		//Get a Page of Client Telephonies - for Page Listings
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientTelephonies_v1Result> PageClientTelephonies(int page, string filter, string sortField, int sortOrder)
		{
			//get a page of records
			var result = db.spDesktopDataAdmin_SelectClientTelephonies_v1(filter, sortField, sortOrder, page).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientTelephonies_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}
		
		//Get one ClientTelephony
        public ClientTelephony GetClientTelephony(int id)
        {
            return db.ClientTelephonies.SingleOrDefault(c => c.ClientTelephonyId == id);
        }

        //Add Data From Linked Tables for Display
        public void EditForDisplay(ClientTelephony clientTelephony)
        {
			//Hierarchy
			if(clientTelephony.HierarchyType == "ClientTopUnit") {
				ClientTopUnitRepository clientTopUnitRepository = new ClientTopUnitRepository();
				ClientTopUnit clientTopUnit = clientTopUnitRepository.GetClientTopUnit(clientTelephony.HierarchyItem);
				if (clientTopUnit != null)
				{
					clientTelephony.HierarchyName = clientTopUnit.ClientTopUnitName;
				}
			}
			else if (clientTelephony.HierarchyType == "ClientSubUnit")
			{
				ClientSubUnitRepository clientSubUnitRepository = new ClientSubUnitRepository();
				ClientSubUnit clientSubUnit = clientSubUnitRepository.GetClientSubUnit(clientTelephony.HierarchyItem);
				if (clientSubUnit != null)
				{
					clientTelephony.HierarchyName = clientSubUnit.ClientSubUnitName;
				}
			}

			//MainNumberFlag
			clientTelephony.MainNumberFlagNullable = (clientTelephony.MainNumberFlag == true) ? true : false;
		}

		//Get a list of Hierarchy Types
		public IEnumerable<SelectListItem> GetAllHierarchyTypes() {
			
			var list = new SelectList(
				new[]
                {
                    new { Value = "ClientTopUnit", Text = "ClientTopUnit" },
                    new { Value = "ClientSubUnit", Text = "ClientSubUnit" }
                },
				"Value", "Text", 0);
			
			return list;
		}   

		//Add to DB
		public void Add(ClientTelephony clientTelephony)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_InsertClientTelephony_v1(
				clientTelephony.ClientTelephonyDescription,
				clientTelephony.PhoneNumber,
				clientTelephony.CountryCode,
				clientTelephony.HierarchyType,
				clientTelephony.HierarchyItem,
				clientTelephony.MainNumberFlagNullable,
				clientTelephony.TelephonyTypeId,
				clientTelephony.TravelerBackOfficeTypeCode,
				clientTelephony.ExpiryDate,
				clientTelephony.InternationalPrefixCode,
				clientTelephony.PhoneNumberwithInternationalPrefixCode,
                clientTelephony.CallerEnteredDigitDefinitionTypeId,
				clientTelephony.ClientSnSDescription,
				clientTelephony.ClientSnSButtonText,
                adminUserGuid
			);
		}

		//Update in DB
		public void Update(ClientTelephony clientTelephony)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_UpdateClientTelephony_v1(
				clientTelephony.ClientTelephonyId,
				clientTelephony.ClientTelephonyDescription,
				clientTelephony.PhoneNumber,
				clientTelephony.CountryCode,
				clientTelephony.HierarchyType,
				clientTelephony.HierarchyItem,
				clientTelephony.MainNumberFlagNullable,
				clientTelephony.TelephonyTypeId,
				clientTelephony.TravelerBackOfficeTypeCode,
				clientTelephony.ExpiryDate,
				clientTelephony.InternationalPrefixCode,
				clientTelephony.PhoneNumberwithInternationalPrefixCode,
				clientTelephony.CallerEnteredDigitDefinitionTypeId,
				clientTelephony.ClientSnSDescription,
				clientTelephony.ClientSnSButtonText,
                adminUserGuid,
				clientTelephony.VersionNumber
			);
		}

		//Delete in DB
		public void Delete(ClientTelephony clientTelephony)
		{
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];

			db.spDesktopDataAdmin_DeleteClientTelephony_v1(
				clientTelephony.ClientTelephonyId,
				adminUserGuid,
				clientTelephony.VersionNumber
			);
		}
    }
}
