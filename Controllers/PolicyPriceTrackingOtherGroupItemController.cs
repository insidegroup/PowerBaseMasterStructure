using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class PolicyPriceTrackingOtherGroupItemController : Controller
    {
        //main repositories
        PolicyPriceTrackingOtherGroupItemRepository PolicyPriceTrackingOtherGroupItemRepository = new PolicyPriceTrackingOtherGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();

        //GET: A list of items
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
			PolicyPriceTrackingOtherGroupItemsVM PolicyPriceTrackingOtherGroupItemsVM = new PolicyPriceTrackingOtherGroupItemsVM();
			
			//Check Parent Exists
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id);
            if (policyGroup == null)
            {
                return View("Error");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(id))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
			PolicyPriceTrackingOtherGroupItemsVM.PolicyGroup = policyGroup;

            //SortField + SortOrder settings
            if (string.IsNullOrEmpty(sortField))
            {
				sortField = "Label";
            }
            ViewData["CurrentSortField"] = sortField;

            if (sortOrder == 1)
            {
                ViewData["NewSortOrder"] = 0;
                ViewData["CurrentSortOrder"] = 1;
            }
            else
            {
                ViewData["NewSortOrder"] = 1;
                ViewData["CurrentSortOrder"] = 0;
            }

			PolicyPriceTrackingOtherGroupItemsVM.PolicyPriceTrackingOtherGroupItems = PolicyPriceTrackingOtherGroupItemRepository.GetPolicyPriceTrackingOtherGroupItems(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);

            return View(PolicyPriceTrackingOtherGroupItemsVM);
        }
    }
}
