using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyMessageGroupItemAirVM : CWTBaseViewModel
   {
        public PolicyMessageGroupItemAir PolicyMessageGroupItemAir { get; set; }
        public int PolicyGroupId { get; set; }
        public string PolicyGroupName { get; set; }
        public PolicyRouting PolicyRouting { get; set; }

        public PolicyMessageGroupItemAirVM()
        {

        }
        public PolicyMessageGroupItemAirVM(PolicyMessageGroupItemAir policyMessageGroupItemAir, int policyGroupId, string policyGroupName, PolicyRouting policyRouting)
        {
            PolicyMessageGroupItemAir = policyMessageGroupItemAir;
            PolicyGroupId = policyGroupId;
            PolicyGroupName = policyGroupName;
            PolicyRouting = policyRouting;
        }
    }
}