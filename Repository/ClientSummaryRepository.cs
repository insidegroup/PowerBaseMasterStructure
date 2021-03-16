using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;

namespace CWTDesktopDatabase.Repository
{
    public class ClientSummaryRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());
        //private string groupName = "Client Detail";



        //List of ClientTopUnits based on Search
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSummaryClientTopUnits_v1Result> GetClientSummaryTopUnits(int page, string filter)
        {
            //sanitise SQL
            CWTValidation cwtValidation = new CWTValidation();
            filter = cwtValidation.SanitiseSQL(filter); //DMC JUne 2013 - to check if this is needed

            //query db
            var result = db.spDesktopDataAdmin_SelectClientSummaryClientTopUnits_v1(filter, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSummaryClientTopUnits_v1Result>(result, page, totalRecords);
            return paginatedView;


        }

        //SubUnits+associated data for a TopUnit
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSummaryClientTopUnitClientSubUnits_v1Result> GetClientSummaryClientSubUnitMatrix(string clientTopUnitId, int page)
        {

            //query db
            var result = db.spDesktopDataAdmin_SelectClientSummaryClientTopUnitClientSubUnits_v1(clientTopUnitId, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSummaryClientTopUnitClientSubUnits_v1Result>(result, page, totalRecords);
            return paginatedView;


        }


        //ClientAccounts+associated data for a SubUnit
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSummaryClientSubUnitClientAccounts_v1Result> GetClientSummaryClientAccountMatrix(string clientSubUnitId, int page)
        {

            //query db
            var result = db.spDesktopDataAdmin_SelectClientSummaryClientSubUnitClientAccounts_v1(clientSubUnitId, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSummaryClientSubUnitClientAccounts_v1Result>(result, page, totalRecords);
            return paginatedView;


        }



        /*
            
        //List of All Items - Sortable
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSummaryViewClientAccountName_v1Result> GetSummaryByClientAccount(int page, string filter, string sortField, int sortOrder)
        {
            bool boolSortOrder = Convert.ToBoolean(sortOrder);
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientSummaryViewClientAccountName_v1(page, sortField, boolSortOrder, filter).ToList();
            int totalRecords = 0;

            if (page == 1)
            {
                totalRecords = 1000;//db.fnDesktopDataAdmin_GetClientSummaryViewCount_v1(filter) ?? 0;
                HttpContext.Current.Session["RecordCount"] = totalRecords;
            }
            else
            {
                totalRecords = (int)HttpContext.Current.Session["RecordCount"];
            }

            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSummaryViewClientAccountName_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //List of All Items - Sortable
        public CWTPaginatedList<spDesktopDataAdmin_SelectClientSummaryViewClientSubUnit_v1Result> GetSummaryByClientSubUnit(int page, string filter, string sortField, int sortOrder)
        {
            bool boolSortOrder = Convert.ToBoolean(sortOrder);
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectClientSummaryViewClientSubUnit_v1(page, sortField, boolSortOrder, filter).ToList();
            int totalRecords = 0;

            if (page == 1)
            {
                totalRecords = 1000;////db.fnDesktopDataAdmin_GetClientSummaryViewCount_v1(filter) ?? 0;
                HttpContext.Current.Session["RecordCount"] = totalRecords;
            }
            else
            {
                totalRecords = (int)HttpContext.Current.Session["RecordCount"];
            }

            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectClientSummaryViewClientSubUnit_v1Result>(result, page, totalRecords);
            return paginatedView;

        }

        //Full Summary - Sortable
        public IQueryable<fnDesktopDataAdmin_SelectClientSummaryView_v1Result> GetSummaryX(string filter, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectClientSummaryView_v1().OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectClientSummaryView_v1().OrderBy(sortField).Where(c => (
                        c.ClientAccountName.Contains(filter) ||
                        c.ClientTopUnitName.Contains(filter) ||
                        c.ClientSubUnitDisplayName.Contains(filter))
                      );
            }
        }

        //TopUnit Summary- Sortable
        public IQueryable<fnDesktopDataAdmin_SelectClientTopUnitSummaryView_v1Result> GetTopUnitSummary(string id, string filter, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectClientTopUnitSummaryView_v1(id).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectClientTopUnitSummaryView_v1(id).OrderBy(sortField).Where(c => (
                        c.ClientTopUnitName.Contains(filter) ||
                        c.ClientSubUnitDisplayName.Contains(filter))
                      );
            }
        }
        
        //TopUnit Summary- Sortable
        public IQueryable<fnDesktopDataAdmin_SelectClientSubUnitSummaryView_v1Result> GetSubUnitSummary(string id, string filter, string sortField, int sortOrder)
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
                return db.fnDesktopDataAdmin_SelectClientSubUnitSummaryView_v1(id).OrderBy(sortField);
            }
            else
            {
                return db.fnDesktopDataAdmin_SelectClientSubUnitSummaryView_v1(id).OrderBy(sortField).Where(c => (
                        c.ClientAccountName.Contains(filter) ||
                        c.ClientTopUnitName.Contains(filter) ||
                        c.ClientSubUnitDisplayName.Contains(filter))
                      );
            }
        }
        */
    }
}
