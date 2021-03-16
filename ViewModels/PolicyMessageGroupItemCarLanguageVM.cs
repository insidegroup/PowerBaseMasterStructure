using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyMessageGroupItemCarLanguageVM : CWTBaseViewModel
    {
        public PolicyMessageGroupItemLanguage PolicyMessageGroupItemLanguage { get; set; }
        public IEnumerable<SelectListItem> PolicyMessageGroupItemLanguages { get; set; }
        public string PolicyGroupName { get; set; }
        public int PolicyGroupId { get; set; }
        public int PolicyMessageGroupItemId { get; set; }
        public string PolicyMessageGroupItemName { get; set; }
        public string ProductName { get; set; }
        public string PolicyLocationName{ get; set; }


        public PolicyMessageGroupItemCarLanguageVM()
        {
        }
        public PolicyMessageGroupItemCarLanguageVM(PolicyMessageGroupItemLanguage policyMessageGroupItemLanguage, IEnumerable<SelectListItem> policyMessageGroupItemLanguages, string policyGroupName,
            int policyGroupId, string policyMessageGroupItemName, string productName, string policyLocationName, int policyMessageGroupItemId)
        {
            PolicyMessageGroupItemLanguage = policyMessageGroupItemLanguage;
            PolicyMessageGroupItemLanguages = policyMessageGroupItemLanguages;
            PolicyGroupName = policyGroupName;
            PolicyGroupId = policyGroupId;
            PolicyMessageGroupItemName = policyMessageGroupItemName;
            PolicyMessageGroupItemId = policyMessageGroupItemId;
            ProductName = productName;
            PolicyLocationName = policyLocationName;
        }
    }
}