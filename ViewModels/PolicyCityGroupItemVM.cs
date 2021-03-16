using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CWTDesktopDatabase.Models;
using System.Web.Mvc;

namespace CWTDesktopDatabase.ViewModels
{
    public class PolicyCityGroupItemVM : CWTBaseViewModel
    {
        public PolicyGroup PolicyGroup { get; set; }
        public PolicyCityGroupItem PolicyCityGroupItem { get; set; }
        public IEnumerable<SelectListItem> PolicyCityStatuses { get; set; }
 
        public PolicyCityGroupItemVM()
        {
        }
        public PolicyCityGroupItemVM(PolicyGroup policyGroup, PolicyCityGroupItem policyCityGroupItem, IEnumerable<SelectListItem> policyCityStatuses)
        {
            PolicyGroup = policyGroup;
            PolicyCityGroupItem = policyCityGroupItem;
            PolicyCityStatuses = policyCityStatuses;
        }
    }
}
