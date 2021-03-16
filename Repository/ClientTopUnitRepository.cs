
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using System.Web.Security;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ClientTopUnitRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());


        //List of ClientTopUnits based on Search
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnits_v1Result> GetClientTopUnits(string filter, int page)
        {

            //sanitise SQL	
            //CWTValidation cwtValidation = new CWTValidation();
            //filter = cwtValidation.SanitiseSQL(filter);

            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientTopUnits_v1(filter, adminUserGuid, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnits_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        /*
        //List of ClientAccounts in a ClientSubUnit - Not Sortable
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnits_v1Result> GetClientTopUnits(string filter, int page )
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            int totalRecords = 0;

            //Total Records is calculated on page 1, then stored in cookie
            CWTPagingCookie cwtPagingCookie = new CWTPagingCookie();
            if (page == 1 || !cwtPagingCookie.CookieExists())
            {
                //total records if all were returned
                totalRecords = (int)db.fnDesktopdataAdmin_GetClientTopUnitRecordCount_v1();
                cwtPagingCookie.AddNameValuePairToCookie("ClientAccount", totalRecords.ToString());
            }
            else
            {
                string keyValue = cwtPagingCookie.GetKeyValueFromCookie("ClientAccount");
                if (keyValue == null)
                {
                    //total records if all were returned
                    totalRecords = (int)db.fnDesktopdataAdmin_GetClientTopUnitRecordCount_v1();
                    cwtPagingCookie.AddNameValuePairToCookie("ClientAccount", totalRecords.ToString());
                }
                else
                {
                    totalRecords = Convert.ToInt32(keyValue);
                }

            }

            //get page of results
           var result = db.spDesktopDataAdmin_SelectClientTopUnits_v1(filter, adminUserGuid, page).ToList();
            //int totalRecords = 0;
            //if (result != null){
            //    totalRecords = (int)result.First().TotalRecords;
            //}

            //total records if all were returned
            //int totalRecords = (int)db.fnDesktopdataAdmin_GetClientSubUnitClientAccountsRecordCount_v1(id);

            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnits_v1Result>(result, page, totalRecords);
            return paginatedView;

        }
*/
        //List of All Items - Sortable
        public IQueryable<ClientTopUnit> GetClientTopUnitsOLD(string filter, string sortField, int sortOrder)
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
            if (filter == "")
            {
                return db.ClientTopUnits.OrderBy(sortField);
            }
            else
            {
                return db.ClientTopUnits.OrderBy(sortField).Where(c => c.ClientTopUnitName.Contains(filter));
            }
        }

        //Get one Item
        public ClientTopUnit GetClientTopUnit(string clientTopUnitGuid)
        {
            return db.ClientTopUnits.SingleOrDefault(c => c.ClientTopUnitGuid == clientTopUnitGuid);
        }

		//Queryable List of All SubUnits of a TopUnit - Sortable
		public CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitSubUnitsItems_v1Result> GetClientSubUnits(string clientTopUnitGuid, int page, string sortField, int sortOrder, string filter)
		{
			//get a page of records
			string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
			var result = db.spDesktopDataAdmin_SelectClientTopUnitSubUnitsItems_v1(clientTopUnitGuid, sortField, sortOrder, page, filter, adminUserGuid).ToList();

			//total records for paging
			int totalRecords = 0;
			if (result.Count() > 0)
			{
				totalRecords = (int)result.First().RecordCount;
			}

			//put into page object
			var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientTopUnitSubUnitsItems_v1Result>(result, page, totalRecords);

			//return to user
			return paginatedView;
		}

        //List of All SubUnits of a TopUnit - NonSortable
        public List<spDesktopDataAdmin_SelectClientTopUnitSubUnits_v1Result> GetClientTopUnitSubUnits(string clientTopUnitGuid)
        {

            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            return db.spDesktopDataAdmin_SelectClientTopUnitSubUnits_v1(clientTopUnitGuid, adminUserGuid).ToList();
        }

        //Add Data From Linked Tables for Display
        //public void EditGroupForDisplay(ClientTopUnit clientTopUnit)
        //{
        //    PortraitStatusRepository portraitStatusRepository = new PortraitStatusRepository();
        //    PortraitStatus portraitStatus = new PortraitStatus();

            //portraitStatus = portraitStatusRepository.GetPortraitStatus(clientTopUnit.PortraitStatusId);
            //if (portraitStatus != null)
            //{
            //    clientTopUnit.PortraitStatus = portraitStatus.PortraitStatusDescription;
            //}
           

        //}
    }
}
