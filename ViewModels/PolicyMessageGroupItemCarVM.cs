using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyMessageGroupItemCarVM : CWTBaseViewModel
   {
        public PolicyMessageGroupItemCar PolicyMessageGroupItemCar { get; set; }
        public int PolicyGroupId { get; set; }
        public string PolicyGroupName { get; set; }
        public IEnumerable<SelectListItem> PolicyLocations { get; set; }

        public PolicyMessageGroupItemCarVM()
        {

        }
        public PolicyMessageGroupItemCarVM(PolicyMessageGroupItemCar policyMessageGroupItemCar, int policyGroupId, string policyGroupName, IEnumerable<SelectListItem> policyLocations)
        {
            PolicyMessageGroupItemCar = policyMessageGroupItemCar;
            PolicyGroupId = policyGroupId;
            PolicyGroupName = policyGroupName;
            PolicyLocations = policyLocations;
        }
    }
}