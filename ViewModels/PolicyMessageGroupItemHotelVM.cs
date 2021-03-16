using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyMessageGroupItemHotelVM : CWTBaseViewModel
   {
        public PolicyMessageGroupItemHotel PolicyMessageGroupItemHotel { get; set; }
        public int PolicyGroupId { get; set; }
        public string PolicyGroupName { get; set; }
        public IEnumerable<SelectListItem> PolicyLocations { get; set; }

        public PolicyMessageGroupItemHotelVM()
        {

        }
        public PolicyMessageGroupItemHotelVM(PolicyMessageGroupItemHotel policyMessageGroupItemHotel, int policyGroupId, string policyGroupName, IEnumerable<SelectListItem> policyLocations)
        {
            PolicyMessageGroupItemHotel = policyMessageGroupItemHotel;
            PolicyGroupId = policyGroupId;
            PolicyGroupName = policyGroupName;
            PolicyLocations = policyLocations;
        }
    }
}