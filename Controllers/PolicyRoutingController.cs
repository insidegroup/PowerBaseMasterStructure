using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using System.Text.RegularExpressions;

namespace CWTDesktopDatabase.Controllers
{
    [AjaxTimeOutCheck]
    public class PolicyRoutingController : Controller
    {
        //main repository
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();

        //Build PolicyGroupName
        //returns a 4-digit number eg. "0001" used to identify groups with the same properties
        [HttpPost]
        public JsonResult BuildRoutingName(string routingName)
        {
            string result = "";

			//Sanitise User Input - only allow alphanumeric, spaces, underscores, dashes
			Regex input = new Regex(@"/^[\w\-_\s]+$/", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
			routingName = input.Replace(routingName, String.Empty);
			routingName = routingName.Replace("--", "-");

            result = policyRoutingRepository.BuildRoutingName(routingName);

			//Sanitise return value - only allow alphanumeric
			Regex output = new Regex(@"/^[a-zA-Z0-9]+$/", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
			result = output.Replace(result, String.Empty);

            return Json(result);

        }

        [HttpPost]
        public JsonResult AutoCompletePolicyRoutingFromTo(string searchText, int maxResults)
        {
            var result = hierarchyRepository.LookUpPolicyRoutingFromTo(searchText, maxResults);
            return Json(result);
        }

        // POST: FOr Validation of Item
        [HttpPost]
        public JsonResult IsValidPolicyRoutingFromTo(string fromTo)
        {
            var result = hierarchyRepository.IsValidPolicyRoutingFromTo(fromTo);
            return Json(result);
        }

    }
}
