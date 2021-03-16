using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyMessageGroupItemLanguagesVM : CWTBaseViewModel
   {
        public CWTPaginatedList<spDesktopDataAdmin_SelectPolicyMessageGroupItemLanguage_v1Result> PolicyMessageGroupItemLanguages { get; set; }
        public string PolicyMessageGroupItemName { get; set; }
        public int PolicyMessageGroupItemId { get; set; }
        public int PolicyGroupId { get; set; }
        public string PolicyGroupName { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public bool HasWriteAccess { get; set; }

        public PolicyMessageGroupItemLanguagesVM()
        {
            HasWriteAccess = false;
        }
        public PolicyMessageGroupItemLanguagesVM(int productId, string policyMessageGroupItemName, CWTPaginatedList<spDesktopDataAdmin_SelectPolicyMessageGroupItemLanguage_v1Result> policyMessageGroupItemLanguages, int policyMessageGroupItemId, int policyGroupId, string policyGroupName, string productName, bool hasWriteAccess)
        {
            PolicyMessageGroupItemLanguages = policyMessageGroupItemLanguages;
            PolicyMessageGroupItemName = policyMessageGroupItemName;
            PolicyMessageGroupItemId = policyMessageGroupItemId;
            PolicyGroupId = policyGroupId;
            PolicyGroupName = policyGroupName;
            ProductName = productName;
            ProductId = productId;
            HasWriteAccess = hasWriteAccess;
        }
    }
}