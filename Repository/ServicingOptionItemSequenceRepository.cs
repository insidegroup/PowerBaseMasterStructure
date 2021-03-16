using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Xml;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Repository
{
    public class ServicingOptionItemSequenceRepository
    {
        HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //List of PolicyGroupCountries For Sequencing
        public CWTPaginatedList<spDesktopDataAdmin_SelectServicingOptionItemSequences_v1Result> GetServicingOptionItemSequences(int servicingOptionGroupId, int servicingOptionId, int page)
        {
            //query db
            int pageSize = 50;
            var result = db.spDesktopDataAdmin_SelectServicingOptionItemSequences_v1(page, pageSize, servicingOptionGroupId, servicingOptionId).ToList();
            
            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectServicingOptionItemSequences_v1Result>(result, page, totalRecords, pageSize);
            return paginatedView;
        }

        //Update Sequences of ServicingOptionItems
        public void UpdateServicingOptionItemSequences(System.Xml.Linq.XElement xmlElement)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateServicingOptionItemSequences_v1(xmlElement, adminUserGuid);

        }

        /*
        //List of PolicyAirVendorGroupItems For Sequencing
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirVendorGroupItemSequences_v1Result> GetPolicyAirVendorGroupItemSequences(int id, int page)
        {
            //query db
            int pageSize = 50;
            var result = db.spDesktopDataAdmin_SelectPolicyAirVendorGroupItemSequences_v1(id, page, pageSize).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyAirVendorGroupItemSequences_v1Result>(result, page, totalRecords, pageSize);
            return paginatedView;


        }

        //List of PolicyCarVendorGroupItems For Sequencing
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarVendorGroupItemSequences_v1Result> GetPolicyCarVendorGroupItemSequences(int id, int page)
        {
            //query db
            int pageSize = 50;
            var result = db.spDesktopDataAdmin_SelectPolicyCarVendorGroupItemSequences_v1(id, page, pageSize).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCarVendorGroupItemSequences_v1Result>(result, page, totalRecords, pageSize);
            return paginatedView;


        }

        //List of PolicyCountryGroupItems For Sequencing
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCountryGroupItemSequences_v1Result> GetPolicyCountryGroupItemSequences(int id, int page)
        {
            //query db
            int pageSize = 50;
            var result = db.spDesktopDataAdmin_SelectPolicyCountryGroupItemSequences_v1(id, page, pageSize).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyCountryGroupItemSequences_v1Result>(result, page, totalRecords, pageSize);
            return paginatedView;


        }

        //Update Sequences of PolicyCountries
        public void UpdatePolicyCountrySequences(string xmlElement)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyGroupCountrySequences_v1(System.Xml.Linq.XElement.Parse(xmlElement), adminUserGuid);
        }

        //Update Sequences of PolicyAirVendorGroupItems
        public void UpdatePolicyAirVendorGroupItemSequences(System.Xml.Linq.XElement xmlElement)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyAirVendorGroupItemSequences_v1(xmlElement, adminUserGuid);

        }

        //Update Sequences of PolicyCarVendorGroupItems
        public void UpdatePolicyCarVendorGroupItemSequences(string xmlElement)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyCarVendorGroupItemSequences_v1(System.Xml.Linq.XElement.Parse(xmlElement), adminUserGuid);
        }

        //Update Sequences of PolicyCountryGroupItems
        public void UpdatePolicyCountryGroupItemSequences(string xmlElement)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdatePolicyCountryGroupItemSequences_v1(System.Xml.Linq.XElement.Parse(xmlElement), adminUserGuid);
        }
 */
    }
}
