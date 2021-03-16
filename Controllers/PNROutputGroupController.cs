using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;
using CWTDesktopDatabase.ViewModels;

namespace CWTDesktopDatabase.Controllers
{
    public class PNROutputGroupController : Controller
    {
        //main repository
        PNROutputGroupRepository pnrOutputGroupRepository = new PNROutputGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        private string groupName = "PNR Output";

        // GET: /ListUnDeleted
        public ActionResult ListUnDeleted(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (string.IsNullOrEmpty(sortField))
            {
                sortField = "PNROutputGroupName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
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

			if (pnrOutputGroupRepository == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}

			var cwtPaginatedList = pnrOutputGroupRepository.PagePNROutputGroups(false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

			if (cwtPaginatedList == null)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("Error");
			}
			
			//return items
            return View(cwtPaginatedList);   
        }

        // GET: /ListDeleted
        public ActionResult ListDeleted(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            if (sortField != "HierarchyType" && sortField != "EnabledDate")
            {
                sortField = "PNROutputGroupName";
            }
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
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

            //return items
            var cwtPaginatedList = pnrOutputGroupRepository.PagePNROutputGroups(true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);   
        }

        // GET: /ListOrphaned
        public ActionResult ListOrphaned(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            sortField = "PNROutputGroupName";
            ViewData["CurrentSortField"] = sortField;

            //SortOrder
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

            //return items
            var cwtPaginatedList = pnrOutputGroupRepository.PageOrphanedPNROutputGroups(  page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /View
        public ActionResult View(int id)
        {
            PNROutputGroup group = new PNROutputGroup();
            group = pnrOutputGroupRepository.GetGroup(id);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            pnrOutputGroupRepository.EditGroupForDisplay(group);
            return View(group);
        }    

        [HttpPost]
        public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 15;
            var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
            return Json(result);
        }

        // GET: /HierarchySearch
        public ActionResult HierarchySearch(int id, string p, string t, string h, string filterHierarchyCSUSearchText = "", string filterHierarchyTTSearchText = "")
        {
            PNROutputGroup pnrOutputGroup = new PNROutputGroup();
            pnrOutputGroup = pnrOutputGroupRepository.GetGroup(id);

            //Check Exists
            if (pnrOutputGroup == null)
            {
                ViewData["ActionMethod"] = "HierarchySearchGet";
                return View("RecordDoesNotExistError");
            }

            string filterHierarchySearchProperty = p;
            string filterHierarchySearchText = t;
            string filterHierarchyType = h;

            PNROutputGroupHierarchySearchVM hierarchySearchVM = new PNROutputGroupHierarchySearchVM();
            hierarchySearchVM.GroupId = id;
            hierarchySearchVM.GroupType = groupName;
            hierarchySearchVM.PNROutputGroup = pnrOutputGroup;
            hierarchySearchVM.LinkedHierarchies = pnrOutputGroupRepository.PNROutputGroupLinkedHierarchies(id, filterHierarchyType);
            hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
            hierarchySearchVM.FilterHierarchySearchText = filterHierarchySearchText;
            hierarchySearchVM.LinkedHierarchiesTotal = pnrOutputGroupRepository.CountPNROutputGroupLinkedHierarchies(id); ;
            hierarchySearchVM.AvailableHierarchyTypeDisplayName = pnrOutputGroupRepository.getAvailableHierarchyTypeDisplayName(filterHierarchySearchProperty);

            if (filterHierarchySearchProperty == null)
            {
                hierarchySearchVM.AvailableHierarchies = null;
            }
            else
            {
                hierarchySearchVM.AvailableHierarchies = pnrOutputGroupRepository.PNROutputGroupAvailableHierarchies(id, filterHierarchySearchProperty, filterHierarchySearchText, filterHierarchyCSUSearchText, filterHierarchyTTSearchText);
            }

            RolesRepository rolesRepository = new RolesRepository();
            hierarchySearchVM.HasWriteAccess = rolesRepository.HasWriteAccessToPNROutputGroup(id);

            hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
            hierarchySearchVM.HierarchyPropertyOptions = pnrOutputGroupRepository.GetHierarchyPropertyOptions(groupName, hierarchySearchVM.FilterHierarchySearchProperty);


            return View(hierarchySearchVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HierarchySearch(int groupId, string filterHierarchySearchProperty, string filterHierarchySearchText, string filterHierarchyCSUSearchText = "", string filterHierarchyTTSearchText = "")
        {

            PNROutputGroup pnrOutputGroup = new PNROutputGroup();
            pnrOutputGroup = pnrOutputGroupRepository.GetGroup(groupId);

            //Check Exists
            if (pnrOutputGroup == null)
            {
                ViewData["ActionMethod"] = "HierarchySearchGet";
                return View("RecordDoesNotExistError");
            }

            PNROutputGroupHierarchySearchVM hierarchySearchVM = new PNROutputGroupHierarchySearchVM();
            hierarchySearchVM.GroupId = groupId;
            hierarchySearchVM.GroupType = groupName;
            hierarchySearchVM.PNROutputGroup = pnrOutputGroup;
            hierarchySearchVM.LinkedHierarchies = pnrOutputGroupRepository.PNROutputGroupLinkedHierarchies(groupId, pnrOutputGroupRepository.getHierarchyType(filterHierarchySearchProperty));
            hierarchySearchVM.AvailableHierarchies = pnrOutputGroupRepository.PNROutputGroupAvailableHierarchies(groupId, filterHierarchySearchProperty, filterHierarchySearchText, filterHierarchyCSUSearchText, filterHierarchyTTSearchText);
            hierarchySearchVM.AvailableHierarchyTypeDisplayName = pnrOutputGroupRepository.getAvailableHierarchyTypeDisplayName(filterHierarchySearchProperty);

            if (filterHierarchySearchProperty == null)
            {
                filterHierarchySearchProperty = "ClientSubUnitName";
            }
            hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
            hierarchySearchVM.FilterHierarchySearchText = filterHierarchySearchText;
            hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;

            RolesRepository rolesRepository = new RolesRepository();
            hierarchySearchVM.HasWriteAccess = rolesRepository.HasWriteAccessToPNROutputGroup(groupId);

            hierarchySearchVM.HierarchyPropertyOptions = pnrOutputGroupRepository.GetHierarchyPropertyOptions(groupName, hierarchySearchVM.FilterHierarchySearchProperty);
            hierarchySearchVM.LinkedHierarchiesTotal = pnrOutputGroupRepository.CountPNROutputGroupLinkedHierarchies(groupId);

            return View(hierarchySearchVM);
        }

        // POST: /AddRemoveHierarchy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRemoveHierarchy(PNROutputGroupHierarchyVM groupHierarchyVM)
        {
            //Get Item From Database
            PNROutputGroup group = new PNROutputGroup();
            group = pnrOutputGroupRepository.GetGroup(groupHierarchyVM.GroupId);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "AddRemoveHierarchyPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPNROutputGroup(groupHierarchyVM.GroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Item
            try
            {
                pnrOutputGroupRepository.UpdateLinkedHierarchy(groupHierarchyVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/PNROutputGroup.mvc/HierarchySearch/" + group.PNROutputGroupId;
                    return View("VersionError");
                }

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("HierarchySearch", new
            {
                id = group.PNROutputGroupId,
                p = groupHierarchyVM.FilterHierarchySearchProperty,
                t = groupHierarchyVM.FilterHierarchySearchText,
                h = pnrOutputGroupRepository.getHierarchyType(groupHierarchyVM.FilterHierarchySearchProperty),
                filterHierarchyCSUSearchText = groupHierarchyVM.FilterHierarchyCSUSearchText,
                filterHierarchyTTSearchText = groupHierarchyVM.FilterHierarchyTTSearchText
            });
        }
    }
}
