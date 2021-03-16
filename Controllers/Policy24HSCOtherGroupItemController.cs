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
    public class Policy24HSCOtherGroupItemController : Controller
    {
        //main repositories
        Policy24HSCOtherGroupItemRepository policy24HSCOtherGroupItemRepository = new Policy24HSCOtherGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
        PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();

        //GET: A list of items
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
			Policy24HSCOtherGroupItemsVM policy24HSCOtherGroupItemsVM = new Policy24HSCOtherGroupItemsVM();
			
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
			policy24HSCOtherGroupItemsVM.PolicyGroup = policyGroup;

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

			policy24HSCOtherGroupItemsVM.Policy24HSCVendorGroupItems = policy24HSCOtherGroupItemRepository.GetPolicy24HSCOtherGroupItems(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);

            return View(policy24HSCOtherGroupItemsVM);
        }
    }
}
