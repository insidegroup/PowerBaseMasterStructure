using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using System.Linq.Dynamic;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class ConfigurationParameterRepository
    {
        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Get a Page of ContactTypes - for Page Listings
        public CWTPaginatedList<spDesktopDataAdmin_SelectConfigurationParameters_v1Result> PageConfigurationParameters(int page, string filter, string sortField, int sortOrder)
        {
            //get a page of records
            var result = db.spDesktopDataAdmin_SelectConfigurationParameters_v1(filter, sortField, sortOrder, page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //put into page object
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectConfigurationParameters_v1Result>(result, page, totalRecords);

            //return to user
            return paginatedView;
        }

             
        //get one item
        public ConfigurationParameter GetConfigurationParameter(string configurationParameterName)
        {
            return db.ConfigurationParameters.SingleOrDefault(c => (c.ConfigurationParameterName == configurationParameterName));
        }


        //Update in DB
        public void Edit(ConfigurationParameter configurationParameter)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            
            db.spDesktopDataAdmin_UpdateConfigurationParameter_v1(
                configurationParameter.ConfigurationParameterName,
                configurationParameter.ConfigurationParameterValue,
                adminUserGuid,
                configurationParameter.VersionNumber
            );
        }
    }
}
