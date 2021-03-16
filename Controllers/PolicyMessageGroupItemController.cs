using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Controllers
{
    public class PolicyMessageGroupItemController : Controller
    {
        //main repositories
        PolicyMessageGroupItemRepository policyMessageGroupItemRepository = new PolicyMessageGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET: A list of PolicyMessageGroup Items for this PolicyGroup
        public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
        {
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id); ;

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }


            //SortField + SortOrder settings
            if (sortField != "ProductName" && sortField != "SupplierName" && sortField != "PolicyMessageGroupItemName")
            {
                sortField = "RoutingOrLocationName";
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


             PolicyMessageGroupItemsVM policyMessageGroupItemsVM = new PolicyMessageGroupItemsVM();
            policyMessageGroupItemsVM.PolicyMessageGroupItems = policyMessageGroupItemRepository.GetPolicyMessageGroupItems(id, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);
            policyMessageGroupItemsVM.PolicyGroup = policyGroup;

            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroupMessages(id))
            {
                policyMessageGroupItemsVM.HasWriteAccess = true;
            }

            return View(policyMessageGroupItemsVM);
        }

        public ActionResult SelectType(int id)
        {
            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(id); ;

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "SelectTypeGet";
                return View("RecordDoesNotExistError");
            }

            PolicyMessageGroupItemCreateVM policyMessageGroupItemCreateVM = new PolicyMessageGroupItemCreateVM();
            policyMessageGroupItemCreateVM.PolicyGroup = policyGroup;

            GenericSelectListVM typesSelectListVM = new GenericSelectListVM();
            typesSelectListVM.SelectList = new SelectList(policyMessageGroupItemRepository.SelectPolicyMessageGroupItemTypes().ToList(), "Value", "Name");
            policyMessageGroupItemCreateVM.PolicyMessageGroupItemTypeSelectList = typesSelectListVM;

            return View(policyMessageGroupItemCreateVM);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectType(PolicyMessageGroupItemCreateVM policyMessageGroupItemCreateVM)
        {
            int policyGroupId = policyMessageGroupItemCreateVM.PolicyGroup.PolicyGroupId;
            string type = policyMessageGroupItemCreateVM.PolicyMessageGroupItemType;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyGroupId); ;

            //Check Exists
            if (policyGroup == null)
            {
                ViewData["ActionMethod"] = "SelectTypePost";
                return View("RecordDoesNotExistError");
            }
            

            if(type == "Air"){
                return RedirectToAction("Create","PolicyMessageGroupItemAir", new {id = policyGroupId});
            }else if(type == "Car"){
                 return RedirectToAction("Create","PolicyMessageGroupItemCar", new {id = policyGroupId});
            }

            return RedirectToAction("Create","PolicyMessageGroupItemHotel", new {id = policyGroupId});
            
        }

    }
}
