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
    public class ClientFeeGroupController : Controller
    {
        //main repository
        ClientFeeGroupRepository clientFeeGroupRepository = new ClientFeeGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        private string groupName = "ClientFee";

        // GET: /ListUnDeleted
        public ActionResult ListUnDeleted(string filter, int? page, string sortField, int? sortOrder, int ft = 0)
        {
			//Check Exists
			if (ft == 0)
			{
				ViewData["ActionMethod"] = "ListUnDeletedGet";
				return View("RecordDoesNotExistError");
			}
			
			//SortField
            if (sortField != "HierarchyType" && sortField != "LinkedItemCount")
            {
                sortField = "ClientFeeGroupName";
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
                sortOrder = 0;
            }

            ClientFeeGroupsVM clientFeeGroupsVM = new ClientFeeGroupsVM();
            clientFeeGroupsVM.FeeTypeId = ft;
            clientFeeGroupsVM.FeeTypeDisplayName = clientFeeGroupRepository.FeeTypeDisplayName(ft);
            clientFeeGroupsVM.FeeTypeDisplayNameShort = clientFeeGroupRepository.FeeTypeDisplayNameShort(ft);

            //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                clientFeeGroupsVM.HasDomainWriteAccess = true;
            }

			if (clientFeeGroupRepository != null)
			{
				var clientFeeGroups = clientFeeGroupRepository.PageClientFeeGroups(clientFeeGroupsVM.FeeTypeId, false, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
				
				if (clientFeeGroups != null)
				{
					clientFeeGroupsVM.ClientFeeGroups = clientFeeGroups;
				}
			}

			//return items
			return View(clientFeeGroupsVM);     
        }

        // GET: /ListDeleted
        public ActionResult ListDeleted(int ft, string filter, int? page, string sortField, int? sortOrder)
        {
            //SortField
            if (sortField != "HierarchyType" && sortField != "EnabledDate")
            {
                sortField = "ClientFeeGroupName";
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
                sortOrder = 0;
            }

            ClientFeeGroupsVM clientFeeGroupsVM = new ClientFeeGroupsVM();
            clientFeeGroupsVM.FeeTypeId = ft;
            clientFeeGroupsVM.FeeTypeDisplayName = clientFeeGroupRepository.FeeTypeDisplayName(ft);
            clientFeeGroupsVM.FeeTypeDisplayNameShort = clientFeeGroupRepository.FeeTypeDisplayNameShort(ft);

            //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                clientFeeGroupsVM.HasDomainWriteAccess = true;
            }

            //return items
            clientFeeGroupsVM.ClientFeeGroups = clientFeeGroupRepository.PageClientFeeGroups(clientFeeGroupsVM.FeeTypeId, true, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(clientFeeGroupsVM);     
        }

          // GET: /ListOrphaned
        public ActionResult ListOrphaned(int ft, string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField
            sortField = "ClientFeeGroupName";
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

            ClientFeeGroupsVM clientFeeGroupsVM = new ClientFeeGroupsVM();
            clientFeeGroupsVM.FeeTypeId = ft;
            clientFeeGroupsVM.FeeTypeDisplayName = clientFeeGroupRepository.FeeTypeDisplayName(ft);
            clientFeeGroupsVM.FeeTypeDisplayNameShort = clientFeeGroupRepository.FeeTypeDisplayNameShort(ft);

             //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                clientFeeGroupsVM.HasDomainWriteAccess = true;
            }

            //return items
            clientFeeGroupsVM.ClientFeeGroupsOrphaned = clientFeeGroupRepository.PageOrphanedClientFeeGroups(clientFeeGroupsVM.FeeTypeId,  page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(clientFeeGroupsVM);   
        }
        // GET: /View
        public ActionResult View(int id)
        {
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(id);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            ClientFeeGroupVM clientFeeGroupVM = new ClientFeeGroupVM();

            clientFeeGroupRepository.EditGroupForDisplay(clientFeeGroup);
            clientFeeGroupVM.ClientFeeGroup = clientFeeGroup;
            clientFeeGroupVM.FeeTypeId = (int)clientFeeGroup.FeeTypeId;
            clientFeeGroupVM.FeeTypeDisplayName = clientFeeGroupRepository.FeeTypeDisplayName(clientFeeGroupVM.FeeTypeId);
            clientFeeGroupVM.FeeTypeDisplayNameShort = clientFeeGroupRepository.FeeTypeDisplayNameShort(clientFeeGroupVM.FeeTypeId);

            return View(clientFeeGroupVM);
        }

        // GET: /Create
        public ActionResult Create(int ft)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFeeGroupVM clientFeeGroupVM = new ClientFeeGroupVM();
            clientFeeGroupVM.FeeTypeId = ft;
            clientFeeGroupVM.FeeTypeDisplayName = clientFeeGroupRepository.FeeTypeDisplayName(ft);
            clientFeeGroupVM.FeeTypeDisplayNameShort = clientFeeGroupRepository.FeeTypeDisplayNameShort(ft);

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            clientFeeGroupVM.TripTypes = tripTypesList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            clientFeeGroupVM.HierarchyTypes = hierarchyTypesList;

            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup.FeeTypeId = ft;
            clientFeeGroup.Mandatory = (clientFeeGroup.FeeTypeId == 1 || clientFeeGroup.FeeTypeId == 2);   
            clientFeeGroup.EnabledFlag = true;
            clientFeeGroupVM.ClientFeeGroup = clientFeeGroup;

            return View(clientFeeGroupVM);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClientFeeGroupVM clientFeeGroupVM)
        {
            //Check Access Rights to Domain
            if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //We need to extract group from groupVM
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupVM.ClientFeeGroup;
            if (clientFeeGroup == null)
            {
                ViewData["Message"] = "ValidationError : missing item"; ;
                return View("Error");
            }

            //Check Access Rights to Domain Hierarchy
            if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(clientFeeGroup.HierarchyType, clientFeeGroup.HierarchyCode, clientFeeGroup.SourceSystemCode, groupName))
            {
                ViewData["Message"] = "You cannot add to this hierarchy item";
                return View("Error");
            }

           //Update Model From Form + Validate against DB
	        try
            {
                UpdateModel<ClientFeeGroup>(clientFeeGroup, "ClientFeeGroup");
            }
	        catch
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }
                ViewData["Message"] = "ValidationError : " + n;
                return View("Error");
            }

            //ClientSubUnitTravelerType has extra field
            string hierarchyCode = clientFeeGroup.HierarchyCode;
            if (clientFeeGroup.HierarchyType == "ClientSubUnitTravelerType")
            {
                clientFeeGroup.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
            }

            //Database Update
            try
            {
                clientFeeGroupRepository.Add(clientFeeGroup);
            }
            catch (SqlException ex)
            {
                //Non-Unique Name
                if (ex.Message == "NonUniqueName")
                {
                    return View("NonUniqueNameError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("ListUnDeleted", new { ft = clientFeeGroup.FeeTypeId });
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Get Item From Database
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(id);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFeeGroupVM clientFeeGroupVM = new ClientFeeGroupVM();

            clientFeeGroupRepository.EditGroupForDisplay(clientFeeGroup);
            clientFeeGroupVM.ClientFeeGroup = clientFeeGroup;
            clientFeeGroupVM.FeeTypeId = (int)clientFeeGroup.FeeTypeId;
            clientFeeGroupVM.FeeTypeDisplayName = clientFeeGroupRepository.FeeTypeDisplayName(clientFeeGroupVM.FeeTypeId);
            clientFeeGroupVM.FeeTypeDisplayNameShort = clientFeeGroupRepository.FeeTypeDisplayNameShort(clientFeeGroupVM.FeeTypeId);

            TripTypeRepository tripTypeRepository = new TripTypeRepository();
            SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
            clientFeeGroupVM.TripTypes = tripTypesList;

            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
            clientFeeGroupVM.HierarchyTypes = hierarchyTypesList;

            return View(clientFeeGroupVM);
        }

        // POST: /Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClientFeeGroupVM clientFeeGroupVM)
        {
            //Get Item
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(clientFeeGroupVM.ClientFeeGroup.ClientFeeGroupId);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access Rights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(clientFeeGroup.ClientFeeGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
           //Update Model From Form + Validate against DB
	        try
            {
                UpdateModel<ClientFeeGroup>(clientFeeGroup, "ClientFeeGroup");
            }
	        catch
            {
                string n = "";
                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        n += error.ErrorMessage;
                    }
                }
                ViewData["Message"] = "ValidationError : " + n;
                return View("Error");
            }

            //Dont check for multiple as We are not editing Hierarchy, we have alrady checked access to the item itself
            if (clientFeeGroup.HierarchyType != "Multiple")
            {
                //ClientSubUnitTravelerType has extra field
                string hierarchyCode = clientFeeGroup.HierarchyCode;
                if (clientFeeGroup.HierarchyType == "ClientSubUnitTravelerType")
                {
                    clientFeeGroup.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
                }
                //Check Access Rights to PolicyGroup
                HierarchyRepository hierarchyRepository = new HierarchyRepository();
                if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(clientFeeGroup.HierarchyType, hierarchyCode, clientFeeGroup.SourceSystemCode, groupName))
                {
                    ViewData["Message"] = "You cannot add to this hierarchy item";
                    return View("Error");
                }
            }
           

            //Database Update
            try
            {
                clientFeeGroupRepository.Edit(clientFeeGroup);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientFeeGroup.mvc/Edit/" + clientFeeGroup.ClientFeeGroupId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }
            return RedirectToAction("ListUnDeleted", new { ft = clientFeeGroup.FeeTypeId});
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Get Item From Database
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(id);

            //Check Exists
            if (clientFeeGroup == null || clientFeeGroup.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFeeGroupVM clientFeeGroupVM = new ClientFeeGroupVM();

            clientFeeGroupRepository.EditGroupForDisplay(clientFeeGroup);
            clientFeeGroupVM.ClientFeeGroup = clientFeeGroup;
            clientFeeGroupVM.FeeTypeId = (int)clientFeeGroup.FeeTypeId;
            clientFeeGroupVM.FeeTypeDisplayName = clientFeeGroupRepository.FeeTypeDisplayName(clientFeeGroupVM.FeeTypeId);
            clientFeeGroupVM.FeeTypeDisplayNameShort = clientFeeGroupRepository.FeeTypeDisplayNameShort(clientFeeGroupVM.FeeTypeId);

            return View(clientFeeGroupVM);
        }

        // POST: /Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ClientFeeGroupVM clientFeeGroupVM)
        {
            //Check Valid Item passed in Form       
            if (clientFeeGroupVM.ClientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Get Item From Database
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(clientFeeGroupVM.ClientFeeGroup.ClientFeeGroupId);

            //Check Exists in Databsase
            if (clientFeeGroup == null || clientFeeGroup.DeletedFlag == true)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(clientFeeGroup.ClientFeeGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Form Item
            try
            {
                clientFeeGroupVM.ClientFeeGroup.DeletedFlag = true;
                clientFeeGroupRepository.UpdateGroupDeletedStatus(clientFeeGroupVM.ClientFeeGroup);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientFeeGroup.mvc/Delete/" + clientFeeGroup.ClientFeeGroupId;
                    return View("VersionError");
                }

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }
            return RedirectToAction("ListUnDeleted", new { ft = clientFeeGroup.FeeTypeId });
        }

        // GET: /UnDelete
        public ActionResult UnDelete(int id)
        {
            //Get Item From Database
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(id);

            //Check Exists
            if (clientFeeGroup == null || clientFeeGroup.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(id))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ClientFeeGroupVM clientFeeGroupVM = new ClientFeeGroupVM();

            clientFeeGroupRepository.EditGroupForDisplay(clientFeeGroup);
            clientFeeGroupVM.ClientFeeGroup = clientFeeGroup;
            clientFeeGroupVM.FeeTypeId = (int)clientFeeGroup.FeeTypeId;
            clientFeeGroupVM.FeeTypeDisplayName = clientFeeGroupRepository.FeeTypeDisplayName(clientFeeGroupVM.FeeTypeId);
            clientFeeGroupVM.FeeTypeDisplayNameShort = clientFeeGroupRepository.FeeTypeDisplayNameShort(clientFeeGroupVM.FeeTypeId);

            return View(clientFeeGroupVM);
        }

        // GET: /HierarchySearch
        public ActionResult HierarchySearch(int id, string p, string t, string h)
        {
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(id);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "HierarchySearchGet";
                return View("RecordDoesNotExistError");
            }

            string filterHierarchySearchProperty = p;
            string filterHierarchySearchText = t;
            string filterHierarchyType = h;

            //if (filterHierarchySearchProperty == null)
            //{
            //    filterHierarchySearchProperty = "ClientSubUnitName";
            //}
            HierarchySearchVM hierarchySearchVM = new HierarchySearchVM();
            hierarchySearchVM.GroupId = id;
            hierarchySearchVM.GroupType = groupName;
            hierarchySearchVM.ClientFeeGroup = clientFeeGroup;
            hierarchySearchVM.LinkedHierarchies = clientFeeGroupRepository.ClientFeeGroupLinkedHierarchies(id, filterHierarchyType);
            hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
            hierarchySearchVM.FilterHierarchySearchText = filterHierarchySearchText;
            hierarchySearchVM.LinkedHierarchiesTotal = clientFeeGroupRepository.CountClientFeeGroupLinkedHierarchies(id); ;
            

            if (filterHierarchySearchProperty == null)
            {
                hierarchySearchVM.AvailableHierarchies = null;
            }
            else
            {
                hierarchySearchVM.AvailableHierarchies = clientFeeGroupRepository.ClientFeeGroupAvailableHierarchies(id, filterHierarchySearchProperty,filterHierarchySearchText);
            }
            hierarchySearchVM.FeeTypeDisplayName = clientFeeGroupRepository.FeeTypeDisplayName((int)clientFeeGroup.FeeTypeId);


            hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchiesForHierarchySearch(groupName).ToList(), "Value", "Text", hierarchySearchVM.FilterHierarchySearchProperty);
            
            hierarchySearchVM.HierarchyPropertyOptions = hierarchyTypesList;


            //List<SelectListItem> GetDomainHierarchiesForHierarchySearch
            RolesRepository rolesRepository = new RolesRepository();
            hierarchySearchVM.HasWriteAccess = rolesRepository.HasWriteAccessToClientFeeGroup(id);

            return View(hierarchySearchVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HierarchySearch(int groupId, string filterHierarchySearchText, string filterHierarchySearchProperty)
        {

            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(groupId);

            //Check Exists
            if (clientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "HierarchySearchGet";
                return View("RecordDoesNotExistError");
            }

            HierarchySearchVM hierarchySearchVM = new HierarchySearchVM();
            hierarchySearchVM.GroupId = groupId;
            hierarchySearchVM.GroupType = groupName;
            hierarchySearchVM.ClientFeeGroup = clientFeeGroup;
            hierarchySearchVM.LinkedHierarchies = clientFeeGroupRepository.ClientFeeGroupLinkedHierarchies(groupId, clientFeeGroupRepository.getHierarchyType(filterHierarchySearchProperty));
            hierarchySearchVM.AvailableHierarchies = clientFeeGroupRepository.ClientFeeGroupAvailableHierarchies(groupId, filterHierarchySearchProperty, filterHierarchySearchText);
            hierarchySearchVM.FeeTypeDisplayName = clientFeeGroupRepository.FeeTypeDisplayName((int)clientFeeGroup.FeeTypeId);

            if (filterHierarchySearchProperty == null)
            {
                filterHierarchySearchProperty = "ClientSubUnitName";
            }
            hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
            hierarchySearchVM.FilterHierarchySearchText = filterHierarchySearchText;
            TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
            SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchiesForHierarchySearch(groupName).ToList(), "Value", "Text", hierarchySearchVM.FilterHierarchySearchProperty);

            hierarchySearchVM.HierarchyPropertyOptions = hierarchyTypesList;


            //List<SelectListItem> GetDomainHierarchiesForHierarchySearch
            RolesRepository rolesRepository = new RolesRepository();
            hierarchySearchVM.HasWriteAccess = rolesRepository.HasWriteAccessToClientFeeGroup(groupId);

            return View(hierarchySearchVM);
        }
        // POST: /AddRemoveHierarchy
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRemoveHierarchy(GroupHierarchyVM groupHierarchyVM)
        {
            //Get Item From Database
            ClientFeeGroup group = new ClientFeeGroup();
            group = clientFeeGroupRepository.GetGroup(groupHierarchyVM.GroupId);

            //Check Exists
            if (group == null)
            {
                ViewData["ActionMethod"] = "AddRemoveHierarchyPost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(groupHierarchyVM.GroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Item
            try
            {
                clientFeeGroupRepository.UpdateLinkedHierarchy(groupHierarchyVM);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientFeeGroup.mvc/HierarchySearch/" + group.ClientFeeGroupId;
                    return View("VersionError");
                }

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("HierarchySearch", new { id = group.ClientFeeGroupId, p = groupHierarchyVM.FilterHierarchySearchProperty, t = groupHierarchyVM.FilterHierarchySearchText, h = clientFeeGroupRepository.getHierarchyType(groupHierarchyVM.FilterHierarchySearchProperty) });
        }

        // POST: /UnDelete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnDelete(ClientFeeGroupVM clientFeeGroupVM)
        {
            //Check Valid Item passed in Form       
            if (clientFeeGroupVM.ClientFeeGroup == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Get Item From Database
            ClientFeeGroup clientFeeGroup = new ClientFeeGroup();
            clientFeeGroup = clientFeeGroupRepository.GetGroup(clientFeeGroupVM.ClientFeeGroup.ClientFeeGroupId);

            //Check Exists in Databsase
            if (clientFeeGroup == null || clientFeeGroup.DeletedFlag == false)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }
            //Check Access
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToClientFeeGroup(clientFeeGroup.ClientFeeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Form Item
            try
            {
                clientFeeGroupVM.ClientFeeGroup.DeletedFlag = false;
                clientFeeGroupRepository.UpdateGroupDeletedStatus(clientFeeGroupVM.ClientFeeGroup);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ClientFeeGroup.mvc/UnDelete/" + clientFeeGroup.ClientFeeGroupId;
                    return View("VersionError");
                }

                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("ListUnDeleted", new { ft = clientFeeGroup.FeeTypeId });
        }


        [HttpPost]
        public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
        {
            HierarchyRepository hierarchyRepository = new HierarchyRepository();

            int maxResults = 15;
            var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
            return Json(result);
        }
    }
}
