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
    public class ReasonCodeItemSequenceRepository
    {
        HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //List of PolicyGroupCountries For Sequencing
        public CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeGroupReasonCodeTypeSequences_v1Result> GetReasonCodeItemSequences(int reasonCodeGroupId, int reasonCodeTypeId, int page)
        {
            //query db
            int pageSize = 50;
            var result = db.spDesktopDataAdmin_SelectReasonCodeGroupReasonCodeTypeSequences_v1(reasonCodeGroupId, reasonCodeTypeId, page, pageSize).ToList();
            
            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectReasonCodeGroupReasonCodeTypeSequences_v1Result>(result, page, totalRecords, pageSize);
            return paginatedView;


        }

    

        //Update Sequences of PolicyCountries
        public void UpdateReasonCodeItemSequences(string xmlElement)
        {
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            db.spDesktopDataAdmin_UpdateReasonCodeGroupReasonCodeTypeSequences_v1(System.Xml.Linq.XElement.Parse(xmlElement), adminUserGuid);
        }

      
    }
}
