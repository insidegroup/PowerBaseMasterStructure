using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.Repository
{
    public class PolicyMessageGroupItemRepository
    {

        private HierarchyDC db = new HierarchyDC(Settings.getConnectionString());

        //Sortable List
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyMessageGroupItems_v1Result> GetPolicyMessageGroupItems(int policyGroupID, string filter, string sortField, int sortOrder, int page)
        {
            //query db
            string adminUserGuid = HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
            var result = db.spDesktopDataAdmin_SelectPolicyMessageGroupItems_v1(policyGroupID, adminUserGuid, filter, sortField, Convert.ToBoolean(sortOrder), page).ToList();

            //total records for paging
            int totalRecords = 0;
            if (result.Count() > 0)
            {
                totalRecords = (int)result.First().RecordCount;
            }

            //add paging information and return1
            var paginatedView = new CWTPaginatedList<spDesktopDataAdmin_SelectPolicyMessageGroupItems_v1Result>(result, page, totalRecords);
            return paginatedView;

        }
    
        //Add Data From Linked Tables for Display
        public List<PolicyMessageGroupItemType> SelectPolicyMessageGroupItemTypes()
        {
            List<PolicyMessageGroupItemType> items = new List<PolicyMessageGroupItemType>();
            items.Add(new PolicyMessageGroupItemType{Name = "Air",Value = "Air"});
            items.Add(new PolicyMessageGroupItemType{Name = "Car",Value = "Car"});
            items.Add(new PolicyMessageGroupItemType{Name = "Hotel",Value = "Hotel"});
            return items.ToList();
        }

        public PolicyMessageGroupItem GetPolicyMessageGroupItem(int policyMessageGroupItemId)
        {
            return db.PolicyMessageGroupItems.SingleOrDefault(c => c.PolicyMessageGroupItemId == policyMessageGroupItemId);
        }
    }

   
}