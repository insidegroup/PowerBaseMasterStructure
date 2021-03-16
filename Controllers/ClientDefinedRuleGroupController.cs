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
using System.Xml;
using System.Collections;

namespace CWTDesktopDatabase.Controllers
{
    public class ClientDefinedRuleGroupController : Controller
    {
        //main repository
        ClientDefinedRuleGroupRepository clientDefinedRuleGroupRepository = new ClientDefinedRuleGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
		
		private string groupName = "Client Rules Group Administrator";

        // GET: /ListUnDeleted
        public ActionResult ListUnDeleted(
			string filter, 
			int? page, 
			string sortField, 
			int? sortOrder, 
			string clientTopUnitName, 
			string clientSubUnitName, 
			string clientAccountNumber, 
			string sourceSystemCode, 
			string travelerTypeName, 
			string clientDefinedRuleGroupName)
        {

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
                if (!string.IsNullOrEmpty(filter)) {
                    sortField = "GroupSequenceNumber";
                } else {
                    sortField = "ClientDefinedRuleGroupName";
                }
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

			ClientDefinedRuleGroupsVM clientDefinedRuleGroupsVM = new ClientDefinedRuleGroupsVM();

            //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                clientDefinedRuleGroupsVM.HasDomainWriteAccess = true;
            }

			if (clientDefinedRuleGroupRepository != null)
			{
				var clientDefinedRuleGroups = clientDefinedRuleGroupRepository.PageClientDefinedRuleGroups(
					false, 
					clientTopUnitName, 
					clientSubUnitName, 
					clientAccountNumber, 
					sourceSystemCode, 
					travelerTypeName, 
					clientDefinedRuleGroupName, 
					page ?? 1, 
					filter ?? "", 
					sortField, 
					sortOrder ?? 0
				);
				
				if (clientDefinedRuleGroups != null)
				{
					clientDefinedRuleGroupsVM.ClientDefinedRuleGroups = clientDefinedRuleGroups;
				}
			}

			//Search Fields
			SelectList searchListFilters = new SelectList((IEnumerable)clientDefinedRuleGroupRepository.GetSearchListFilters(), "Key", "Value", filter);
			clientDefinedRuleGroupsVM.SearchListFilters = searchListFilters;
			clientDefinedRuleGroupsVM.Filter = filter ?? "";
			clientDefinedRuleGroupsVM.ClientTopUnitName = clientTopUnitName ?? "";
			clientDefinedRuleGroupsVM.ClientSubUnitName = clientSubUnitName ?? "";
			clientDefinedRuleGroupsVM.ClientAccountNumber = clientAccountNumber ?? "";
			clientDefinedRuleGroupsVM.SourceSystemCode = sourceSystemCode ?? "";
			clientDefinedRuleGroupsVM.TravelerTypeName = travelerTypeName ?? "";

			//Breadcrumb Text
			string searchTerm = "";
			switch (filter)
			{
				case "ClientTopUnit":
					searchTerm = clientTopUnitName;
					break;
				case "ClientSubUnit":
					searchTerm = clientSubUnitName;
					break;
				case "ClientAccount":
					searchTerm = clientAccountNumber + " - " + sourceSystemCode;
					break;
				case "TravelerType":
					searchTerm = clientSubUnitName + " - " + travelerTypeName;
					break;
				case "ClientSubUnitTravelerType":
					searchTerm = clientSubUnitName + " - " + travelerTypeName;
					break;
			}
			clientDefinedRuleGroupsVM.SearchTerm = searchTerm;

			//return items
			return View(clientDefinedRuleGroupsVM);     
        }

		// GET: /ListDeleted
		public ActionResult ListDeleted(
			string filter, 
			int? page, 
			string sortField, 
			int? sortOrder, 
			string clientTopUnitName, 
			string clientSubUnitName, 
			string clientAccountNumber, 
			string sourceSystemCode, 
			string travelerTypeName, 
			string clientDefinedRuleGroupName)
        {

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "ClientDefinedRuleGroupName";
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

			ClientDefinedRuleGroupsVM clientDefinedRuleGroupsVM = new ClientDefinedRuleGroupsVM();

            //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                clientDefinedRuleGroupsVM.HasDomainWriteAccess = true;
            }

			if (clientDefinedRuleGroupRepository != null)
			{
				var clientDefinedRuleGroups = clientDefinedRuleGroupRepository.PageClientDefinedRuleGroups(
					true, 
					clientTopUnitName, 
					clientSubUnitName, 
					clientAccountNumber, 
					sourceSystemCode, 
					travelerTypeName, 
					clientDefinedRuleGroupName, 
					page ?? 1, 
					filter ?? "", 
					sortField, 
					sortOrder ?? 0
				);
				
				if (clientDefinedRuleGroups != null)
				{
					clientDefinedRuleGroupsVM.ClientDefinedRuleGroups = clientDefinedRuleGroups;
				}
			}

			//Search Fields
			SelectList searchListFilters = new SelectList(clientDefinedRuleGroupRepository.GetSearchListFilters().ToList(), filter);
			clientDefinedRuleGroupsVM.SearchListFilters = searchListFilters;
			clientDefinedRuleGroupsVM.Filter = filter ?? "";
			clientDefinedRuleGroupsVM.ClientTopUnitName = clientTopUnitName ?? "";
			clientDefinedRuleGroupsVM.ClientSubUnitName = clientSubUnitName ?? "";
			clientDefinedRuleGroupsVM.ClientAccountNumber = clientAccountNumber ?? "";
			clientDefinedRuleGroupsVM.SourceSystemCode = sourceSystemCode ?? "";
			clientDefinedRuleGroupsVM.TravelerTypeName = travelerTypeName ?? "";
			clientDefinedRuleGroupsVM.ClientDefinedRuleGroupName = clientDefinedRuleGroupName ?? "";

			//return items
			return View(clientDefinedRuleGroupsVM);     
		}

		//  // GET: /ListOrphaned
		//public ActionResult ListOrphaned(int ft, string filter, int? page, string sortField, int? sortOrder)
		//{
		//	//Set Access Rights
		//	ViewData["Access"] = "";
		//	if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
		//	{
		//		ViewData["Access"] = "WriteAccess";
		//	}

		//	//SortField
		//	sortField = "ClientDefinedRuleGroupName";
		//	ViewData["CurrentSortField"] = sortField;

		//	//SortOrder
		//	if (sortOrder == 1)
		//	{
		//		ViewData["NewSortOrder"] = 0;
		//		ViewData["CurrentSortOrder"] = 1;
		//	}
		//	else
		//	{
		//		ViewData["NewSortOrder"] = 1;
		//		ViewData["CurrentSortOrder"] = 0;
		//	}

		//	ClientDefinedRuleGroupsVM clientDefinedRuleGroupsVM = new ClientDefinedRuleGroupsVM();
		//	clientDefinedRuleGroupsVM.FeeTypeId = ft;
		//	clientDefinedRuleGroupsVM.FeeTypeDisplayName = clientDefinedRuleGroupRepository.FeeTypeDisplayName(ft);
		//	clientDefinedRuleGroupsVM.FeeTypeDisplayNameShort = clientDefinedRuleGroupRepository.FeeTypeDisplayNameShort(ft);

		//	 //Set Access Rights
		//	if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
		//	{
		//		clientDefinedRuleGroupsVM.HasDomainWriteAccess = true;
		//	}

		//	//return items
		//	clientDefinedRuleGroupsVM.ClientDefinedRuleGroupsOrphaned = clientDefinedRuleGroupRepository.PageOrphanedClientDefinedRuleGroups(clientDefinedRuleGroupsVM.FeeTypeId,  page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
		//	return View(clientDefinedRuleGroupsVM);   
		//}
		
		// GET: /Create
		
		public ActionResult Create()
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientDefinedRuleGroupVM clientDefinedRuleGroupVM = new ClientDefinedRuleGroupVM();

			//ClientDefinedRuleBusinessEntityCategories
			ClientDefinedRuleBusinessEntityCategoryRepository clientDefinedRuleBusinessEntityCategoryRepository = new ClientDefinedRuleBusinessEntityCategoryRepository();
			SelectList clientDefinedRuleBusinessEntityCategories = new SelectList(clientDefinedRuleBusinessEntityCategoryRepository.GetClientDefinedRuleBusinessEntityCategories().ToList(), "ClientDefinedRuleBusinessEntityCategoryDescription", "ClientDefinedRuleBusinessEntityCategoryDescription");
			clientDefinedRuleGroupVM.ClientDefinedRuleBusinessEntityCategories = clientDefinedRuleBusinessEntityCategories;

			//Trip Types
			TripTypeRepository tripTypeRepository = new TripTypeRepository();
			SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription");
			clientDefinedRuleGroupVM.TripTypes = tripTypesList;

			//Hierarchy
			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName");
			clientDefinedRuleGroupVM.HierarchyTypes = hierarchyTypesList;

			//ClientDefinedRuleGroup
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup.EnabledFlag = true;
			clientDefinedRuleGroupVM.ClientDefinedRuleGroup = clientDefinedRuleGroup;

			ClientDefinedRuleBusinessEntityRepository clientDefinedRuleBusinessEntityRepository = new ClientDefinedRuleBusinessEntityRepository();

			//ClientDefinedRuleLogicBusinessEntities
			SelectList clientDefinedRuleLogicBusinessEntities = new SelectList(clientDefinedRuleBusinessEntityRepository.GetClientDefinedRuleLogicBusinessEntities().ToList(), "ClientDefinedRuleBusinessEntityId", "BusinessEntityDescription");
			clientDefinedRuleGroupVM.ClientDefinedRuleLogicBusinessEntities = clientDefinedRuleLogicBusinessEntities;

			//ClientDefinedRuleResultBusinessEntities
			SelectList clientDefinedRuleResultBusinessEntities = new SelectList(clientDefinedRuleBusinessEntityRepository.GetClientDefinedRuleResultBusinessEntities().ToList(), "ClientDefinedRuleBusinessEntityId", "BusinessEntityDescription");
			clientDefinedRuleGroupVM.ClientDefinedRuleResultBusinessEntities = clientDefinedRuleResultBusinessEntities;

			//ClientDefinedRuleGroupLogic
			List<ClientDefinedRuleGroupLogic> clientDefinedRuleGroupLogics = new List<ClientDefinedRuleGroupLogic>();
			ClientDefinedRuleGroupLogic clientDefinedRuleGroupLogic = new ClientDefinedRuleGroupLogic();
			clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem = new ClientDefinedRuleLogicItem();
			clientDefinedRuleGroupLogics.Add(clientDefinedRuleGroupLogic);
			clientDefinedRuleGroupVM.ClientDefinedRuleGroupLogics = clientDefinedRuleGroupLogics;

			//ClientDefinedRuleGroupResult
			List<ClientDefinedRuleGroupResult> clientDefinedRuleGroupResults = new List<ClientDefinedRuleGroupResult>();
			ClientDefinedRuleGroupResult clientDefinedRuleGroupResult = new ClientDefinedRuleGroupResult();
			clientDefinedRuleGroupResult.ClientDefinedRuleResultItem = new ClientDefinedRuleResultItem();
			clientDefinedRuleGroupResults.Add(clientDefinedRuleGroupResult);
			clientDefinedRuleGroupVM.ClientDefinedRuleGroupResults = clientDefinedRuleGroupResults;
			
			//ClientDefinedRuleWorkflowTriggers
			List<ClientDefinedRuleGroupTrigger> clientDefinedRuleGroupTriggers = new List<ClientDefinedRuleGroupTrigger>();
			ClientDefinedRuleGroupTrigger clientDefinedRuleGroupTrigger = new ClientDefinedRuleGroupTrigger();
			clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger = new ClientDefinedRuleWorkflowTrigger();
			clientDefinedRuleGroupTriggers.Add(clientDefinedRuleGroupTrigger);
			clientDefinedRuleGroupVM.ClientDefinedRuleGroupTriggers = clientDefinedRuleGroupTriggers;

			//ClientDefinedRuleRelationalOperators
			ClientDefinedRuleRelationalOperatorRepository clientDefinedRuleRelationalOperatorRepository = new ClientDefinedRuleRelationalOperatorRepository();
			SelectList clientDefinedRuleRelationalOperators = new SelectList(clientDefinedRuleRelationalOperatorRepository.GetClientDefinedRuleRelationalOperators().ToList(), "ClientDefinedRuleRelationalOperatorId", "RelationalOperatorName");
			clientDefinedRuleGroupVM.ClientDefinedRuleRelationalOperators = clientDefinedRuleRelationalOperators;

			//ClientDefinedRuleWorkflowTriggerStates
			ClientDefinedRuleWorkflowTriggerStateRepository clientDefinedRuleWorkflowTriggerStateRepository = new ClientDefinedRuleWorkflowTriggerStateRepository();
			SelectList clientDefinedRuleWorkflowTriggerStates = new SelectList(clientDefinedRuleWorkflowTriggerStateRepository.GetClientDefinedRuleWorkflowTriggerStates().ToList(), "ClientDefinedRuleWorkflowTriggerStateId", "ClientDefinedRuleWorkflowTriggerStateName");
			clientDefinedRuleGroupVM.ClientDefinedRuleWorkflowTriggerStates = clientDefinedRuleWorkflowTriggerStates;

			//ClientDefinedRuleWorkflowTriggerApplicationModes
			ClientDefinedRuleWorkflowTriggerApplicationModeRepository clientDefinedRuleWorkflowTriggerApplicationModeRepository = new ClientDefinedRuleWorkflowTriggerApplicationModeRepository();
			SelectList clientDefinedRuleWorkflowTriggerApplicationModes = new SelectList(clientDefinedRuleWorkflowTriggerApplicationModeRepository.GetClientDefinedRuleWorkflowTriggerApplicationModes().ToList(), "ClientDefinedRuleWorkflowTriggerApplicationModeId", "ClientDefinedRuleWorkflowTriggerApplicationModeName");
			clientDefinedRuleGroupVM.ClientDefinedRuleWorkflowTriggerApplicationModes = clientDefinedRuleWorkflowTriggerApplicationModes;

			return View(clientDefinedRuleGroupVM);
		}

		// POST: /Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//We need to extract group from groupVM
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupVM.ClientDefinedRuleGroup;
			if (clientDefinedRuleGroup == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Check Access Rights to Domain Hierarchy
			if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(clientDefinedRuleGroup.HierarchyType, clientDefinedRuleGroup.HierarchyCode, clientDefinedRuleGroup.SourceSystemCode, groupName))
			{
				ViewData["Message"] = "You cannot add to this hierarchy item";
				return View("Error");
			}

			//Populate CreateClientDefinedRuleGroupData from FormCollection
			clientDefinedRuleGroupVM = CreateClientDefinedRuleGroupData(clientDefinedRuleGroupVM, formCollection);

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<ClientDefinedRuleGroup>(clientDefinedRuleGroup, "ClientDefinedRuleGroup");
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

			//Database Update
			try
			{
				clientDefinedRuleGroupRepository.Add(clientDefinedRuleGroupVM);
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

			return RedirectToAction("ListUnDeleted");
		}

		// GET: /Edit
		public ActionResult Edit(int id)
		{
			//Get Item From Database
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(id);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientDefinedRuleGroupVM clientDefinedRuleGroupVM = new ClientDefinedRuleGroupVM();

			ClientDefinedRuleBusinessEntityCategoryRepository clientDefinedRuleBusinessEntityCategoryRepository = new ClientDefinedRuleBusinessEntityCategoryRepository();
			SelectList clientDefinedRuleBusinessEntityCategories = new SelectList(clientDefinedRuleBusinessEntityCategoryRepository.GetClientDefinedRuleBusinessEntityCategories().ToList(), "ClientDefinedRuleBusinessEntityCategoryDescription", "ClientDefinedRuleBusinessEntityCategoryDescription", clientDefinedRuleGroup.Category);
			clientDefinedRuleGroupVM.ClientDefinedRuleBusinessEntityCategories = clientDefinedRuleBusinessEntityCategories;

			TripTypeRepository tripTypeRepository = new TripTypeRepository();
			SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription", clientDefinedRuleGroup.TripTypeId);
			clientDefinedRuleGroupVM.TripTypes = tripTypesList;

			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName", clientDefinedRuleGroup.HierarchyItem);
			clientDefinedRuleGroupVM.HierarchyTypes = hierarchyTypesList;

			//ClientDefinedRuleGroup
			clientDefinedRuleGroupRepository.EditGroupForDisplay(clientDefinedRuleGroup);
			clientDefinedRuleGroupVM.ClientDefinedRuleGroup = clientDefinedRuleGroup;

			//ClientDefinedRuleRelationalOperators
			ClientDefinedRuleRelationalOperatorRepository clientDefinedRuleRelationalOperatorRepository = new ClientDefinedRuleRelationalOperatorRepository();
			SelectList clientDefinedRuleRelationalOperators = new SelectList(clientDefinedRuleRelationalOperatorRepository.GetClientDefinedRuleRelationalOperators().ToList(), "ClientDefinedRuleRelationalOperatorId", "RelationalOperatorName");
			clientDefinedRuleGroupVM.ClientDefinedRuleRelationalOperators = clientDefinedRuleRelationalOperators;

			//ClientDefinedRuleWorkflowTriggerStates
			ClientDefinedRuleWorkflowTriggerStateRepository clientDefinedRuleWorkflowTriggerStateRepository = new ClientDefinedRuleWorkflowTriggerStateRepository();
			SelectList clientDefinedRuleWorkflowTriggerStates = new SelectList(clientDefinedRuleWorkflowTriggerStateRepository.GetClientDefinedRuleWorkflowTriggerStates().ToList(), "ClientDefinedRuleWorkflowTriggerStateId", "ClientDefinedRuleWorkflowTriggerStateName");
			clientDefinedRuleGroupVM.ClientDefinedRuleWorkflowTriggerStates = clientDefinedRuleWorkflowTriggerStates;

			//ClientDefinedRuleWorkflowTriggerApplicationModes
			ClientDefinedRuleWorkflowTriggerApplicationModeRepository clientDefinedRuleWorkflowTriggerApplicationModeRepository = new ClientDefinedRuleWorkflowTriggerApplicationModeRepository();
			SelectList clientDefinedRuleWorkflowTriggerApplicationModes = new SelectList(clientDefinedRuleWorkflowTriggerApplicationModeRepository.GetClientDefinedRuleWorkflowTriggerApplicationModes().ToList(), "ClientDefinedRuleWorkflowTriggerApplicationModeId", "ClientDefinedRuleWorkflowTriggerApplicationModeName");
			clientDefinedRuleGroupVM.ClientDefinedRuleWorkflowTriggerApplicationModes = clientDefinedRuleWorkflowTriggerApplicationModes;

			//ClientDefinedRuleGroupLogics
			ClientDefinedRuleGroupLogicRepository clientDefinedRuleGroupLogicRepository = new ClientDefinedRuleGroupLogicRepository();
			List<ClientDefinedRuleGroupLogic> clientDefinedRuleGroupLogics = clientDefinedRuleGroupLogicRepository.GetClientDefinedRuleGroupLogics(clientDefinedRuleGroup.ClientDefinedRuleGroupId);
			if (clientDefinedRuleGroupLogics.Count == 0)
			{
				ClientDefinedRuleGroupLogic clientDefinedRuleGroupLogic = new ClientDefinedRuleGroupLogic();
				clientDefinedRuleGroupLogicRepository.EditForDisplay(clientDefinedRuleGroupLogic); 
				clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem = new ClientDefinedRuleLogicItem();
				clientDefinedRuleGroupLogics.Add(clientDefinedRuleGroupLogic);
			}
			clientDefinedRuleGroupVM.ClientDefinedRuleGroupLogics = clientDefinedRuleGroupLogics;

			//ClientDefinedRuleGroupResult
			ClientDefinedRuleGroupResultRepository clientDefinedRuleGroupResultRepository = new ClientDefinedRuleGroupResultRepository();
			List<ClientDefinedRuleGroupResult> clientDefinedRuleGroupResults = clientDefinedRuleGroupResultRepository.GetClientDefinedRuleGroupResults(clientDefinedRuleGroup.ClientDefinedRuleGroupId);
			if (clientDefinedRuleGroupResults.Count == 0)
			{
				ClientDefinedRuleGroupResult clientDefinedRuleGroupResult = new ClientDefinedRuleGroupResult();
				clientDefinedRuleGroupResultRepository.EditForDisplay(clientDefinedRuleGroupResult);
				clientDefinedRuleGroupResult.ClientDefinedRuleResultItem = new ClientDefinedRuleResultItem();
				clientDefinedRuleGroupResults.Add(clientDefinedRuleGroupResult); 
			} 
			clientDefinedRuleGroupVM.ClientDefinedRuleGroupResults = clientDefinedRuleGroupResults;

			//ClientDefinedRuleGroupTrigger
			ClientDefinedRuleGroupTriggerRepository clientDefinedRuleGroupTriggerRepository = new ClientDefinedRuleGroupTriggerRepository();
			List<ClientDefinedRuleGroupTrigger> clientDefinedRuleGroupTriggers = clientDefinedRuleGroupTriggerRepository.GetClientDefinedRuleGroupTriggers(clientDefinedRuleGroup.ClientDefinedRuleGroupId);
			if (clientDefinedRuleGroupTriggers.Count == 0)
			{
				ClientDefinedRuleGroupTrigger clientDefinedRuleGroupTrigger = new ClientDefinedRuleGroupTrigger();
				clientDefinedRuleGroupTriggerRepository.EditForDisplay(clientDefinedRuleGroupTrigger); 
				clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger = new ClientDefinedRuleWorkflowTrigger();
				clientDefinedRuleGroupTriggers.Add(clientDefinedRuleGroupTrigger);
			}
			clientDefinedRuleGroupVM.ClientDefinedRuleGroupTriggers = clientDefinedRuleGroupTriggers;

			return View(clientDefinedRuleGroupVM);
		}

		// POST: /Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM, FormCollection formCollection)
		{
			//Get Item
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ClientDefinedRuleGroupId);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientDefinedRuleGroup(clientDefinedRuleGroup.ClientDefinedRuleGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Populate CreateClientDefinedRuleGroupData from FormCollection
			clientDefinedRuleGroupVM = CreateClientDefinedRuleGroupData(clientDefinedRuleGroupVM, formCollection);
			
			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<ClientDefinedRuleGroup>(clientDefinedRuleGroup, "ClientDefinedRuleGroup");
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
			if (clientDefinedRuleGroup.HierarchyType != "Multiple")
			{
				//ClientSubUnitTravelerType has extra field
				string hierarchyCode = clientDefinedRuleGroup.HierarchyCode;
				if (clientDefinedRuleGroup.HierarchyType == "ClientSubUnitTravelerType")
				{
					clientDefinedRuleGroup.ClientSubUnitGuid = hierarchyCode;  //ClientSubUnitTravelerType has 2 primarykeys
				}

				//Check Access Rights to PolicyGroup
				HierarchyRepository hierarchyRepository = new HierarchyRepository();
				if (!hierarchyRepository.AdminHasDomainHierarchyWriteAccess(clientDefinedRuleGroup.HierarchyType, hierarchyCode, clientDefinedRuleGroup.SourceSystemCode, groupName))
				{
					ViewData["Message"] = "You cannot add to this hierarchy item";
					return View("Error");
				}
			}

			//Database Update
			try
			{
				clientDefinedRuleGroupRepository.Edit(clientDefinedRuleGroupVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientDefinedRuleGroup.mvc/Edit/" + clientDefinedRuleGroup.ClientDefinedRuleGroupId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListUnDeleted");
		}

		// GET: /View
		public ActionResult View(int id)
		{
			//Get Item From Database
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(id);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedRuleGroupVM clientDefinedRuleGroupVM = new ClientDefinedRuleGroupVM();

			clientDefinedRuleGroupVM = LoadClientDefinedRuleGroupVM(clientDefinedRuleGroupVM, clientDefinedRuleGroup);

			return View(clientDefinedRuleGroupVM);
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(id);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientDefinedRuleGroupVM clientDefinedRuleGroupVM = new ClientDefinedRuleGroupVM();

			clientDefinedRuleGroupVM = LoadClientDefinedRuleGroupVM(clientDefinedRuleGroupVM, clientDefinedRuleGroup);

			return View(clientDefinedRuleGroupVM);
		}

		// POST: /Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM)
		{
			//Check Valid Item passed in Form       
			if (clientDefinedRuleGroupVM.ClientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ClientDefinedRuleGroupId);

			//Check Exists in Databsase
			if (clientDefinedRuleGroup == null || clientDefinedRuleGroup.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientDefinedRuleGroup(clientDefinedRuleGroup.ClientDefinedRuleGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			//Delete Form Item
			try
			{
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.DeletedFlag = true;
				clientDefinedRuleGroupRepository.UpdateGroupDeletedStatus(clientDefinedRuleGroupVM.ClientDefinedRuleGroup);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientDefinedRuleGroup.mvc/Delete/" + clientDefinedRuleGroup.ClientDefinedRuleGroupId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted");
		}

		// GET: /UnDelete
		public ActionResult UnDelete(int id)
		{
			//Get Item From Database
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(id);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			ClientDefinedRuleGroupVM clientDefinedRuleGroupVM = new ClientDefinedRuleGroupVM();

			clientDefinedRuleGroupVM = LoadClientDefinedRuleGroupVM(clientDefinedRuleGroupVM, clientDefinedRuleGroup);

			return View(clientDefinedRuleGroupVM);
		}

		public ClientDefinedRuleGroupVM LoadClientDefinedRuleGroupVM(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM, ClientDefinedRuleGroup clientDefinedRuleGroup)
		{
			ClientDefinedRuleBusinessEntityCategoryRepository clientDefinedRuleBusinessEntityCategoryRepository = new ClientDefinedRuleBusinessEntityCategoryRepository();
			SelectList clientDefinedRuleBusinessEntityCategories = new SelectList(clientDefinedRuleBusinessEntityCategoryRepository.GetClientDefinedRuleBusinessEntityCategories().ToList(), "ClientDefinedRuleBusinessEntityCategoryDescription", "ClientDefinedRuleBusinessEntityCategoryDescription", clientDefinedRuleGroup.Category);
			clientDefinedRuleGroupVM.ClientDefinedRuleBusinessEntityCategories = clientDefinedRuleBusinessEntityCategories;

			TripTypeRepository tripTypeRepository = new TripTypeRepository();
			SelectList tripTypesList = new SelectList(tripTypeRepository.GetAllTripTypes().ToList(), "TripTypeId", "TripTypeDescription", clientDefinedRuleGroup.TripTypeId);
			clientDefinedRuleGroupVM.TripTypes = tripTypesList;

			TripType tripType = new TripType();
			if (clientDefinedRuleGroup.TripTypeId != null)
			{
				tripType = tripTypeRepository.GetTripType(clientDefinedRuleGroup.TripTypeId);
				if (tripType != null)
				{
					clientDefinedRuleGroup.TripType = tripType;
				}
			}

			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchies(groupName).ToList(), "HierarchyLevelTableName", "HierarchyLevelTableName", clientDefinedRuleGroup.HierarchyItem);
			clientDefinedRuleGroupVM.HierarchyTypes = hierarchyTypesList;

			//ClientDefinedRuleGroup
			clientDefinedRuleGroupRepository.EditGroupForDisplay(clientDefinedRuleGroup);
			clientDefinedRuleGroupVM.ClientDefinedRuleGroup = clientDefinedRuleGroup;

			//ClientDefinedRuleBusinessEntities
			ClientDefinedRuleBusinessEntityRepository clientDefinedRuleBusinessEntityRepository = new ClientDefinedRuleBusinessEntityRepository();
			SelectList clientDefinedRuleBusinessEntities = new SelectList(clientDefinedRuleBusinessEntityRepository.GetClientDefinedRuleLogicBusinessEntities().ToList(), "ClientDefinedRuleBusinessEntityId", "BusinessEntityDescription");
			clientDefinedRuleGroupVM.ClientDefinedRuleBusinessEntities = clientDefinedRuleBusinessEntities;

			//ClientDefinedRuleRelationalOperators
			ClientDefinedRuleRelationalOperatorRepository clientDefinedRuleRelationalOperatorRepository = new ClientDefinedRuleRelationalOperatorRepository();
			SelectList clientDefinedRuleRelationalOperators = new SelectList(clientDefinedRuleRelationalOperatorRepository.GetClientDefinedRuleRelationalOperators().ToList(), "ClientDefinedRuleRelationalOperatorId", "RelationalOperatorName");
			clientDefinedRuleGroupVM.ClientDefinedRuleRelationalOperators = clientDefinedRuleRelationalOperators;

			//ClientDefinedRuleGroupLogics
			ClientDefinedRuleGroupLogicRepository clientDefinedRuleGroupLogicRepository = new ClientDefinedRuleGroupLogicRepository();
			List<ClientDefinedRuleGroupLogic> clientDefinedRuleGroupLogics = clientDefinedRuleGroupLogicRepository.GetClientDefinedRuleGroupLogics(clientDefinedRuleGroup.ClientDefinedRuleGroupId);
			clientDefinedRuleGroupVM.ClientDefinedRuleGroupLogics = clientDefinedRuleGroupLogics;

			//ClientDefinedRuleGroupResult
			ClientDefinedRuleGroupResultRepository clientDefinedRuleGroupResultRepository = new ClientDefinedRuleGroupResultRepository();
			List<ClientDefinedRuleGroupResult> clientDefinedRuleGroupResults = clientDefinedRuleGroupResultRepository.GetClientDefinedRuleGroupResults(clientDefinedRuleGroup.ClientDefinedRuleGroupId);
			clientDefinedRuleGroupVM.ClientDefinedRuleGroupResults = clientDefinedRuleGroupResults;

			//ClientDefinedRuleGroupTrigger
			ClientDefinedRuleGroupTriggerRepository clientDefinedRuleGroupTriggerRepository = new ClientDefinedRuleGroupTriggerRepository();
			List<ClientDefinedRuleGroupTrigger> clientDefinedRuleGroupTriggers = clientDefinedRuleGroupTriggerRepository.GetClientDefinedRuleGroupTriggers(clientDefinedRuleGroup.ClientDefinedRuleGroupId);
			clientDefinedRuleGroupVM.ClientDefinedRuleGroupTriggers = clientDefinedRuleGroupTriggers;

			return clientDefinedRuleGroupVM;
		}

		public ClientDefinedRuleGroupVM CreateClientDefinedRuleGroupData(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM, FormCollection formCollection)
		{
			//Create ClientDefinedRuleLogicItems from Post values
			List<ClientDefinedRuleGroupLogic> clientDefinedRuleGroupLogicItems = new List<ClientDefinedRuleGroupLogic>();
			foreach (string key in formCollection)
			{
				if (key.StartsWith("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityName_") && !string.IsNullOrEmpty(formCollection[key]))
				{
					int id = int.Parse(key.Replace("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityName_", ""));

					ClientDefinedRuleGroupLogic clientDefinedRuleGroupLogic = new ClientDefinedRuleGroupLogic();
					ClientDefinedRuleLogicItem clientDefinedRuleLogicItem = new ClientDefinedRuleLogicItem();

					string ClientDefinedRuleBusinessEntityId = string.Format("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId_{0}", id);
					if (formCollection.AllKeys.Contains(ClientDefinedRuleBusinessEntityId) && !string.IsNullOrEmpty(formCollection[ClientDefinedRuleBusinessEntityId]))
					{
						clientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId = int.Parse(formCollection[ClientDefinedRuleBusinessEntityId]);
					}
					
					string ClientDefinedRuleBusinessEntityName = string.Format("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityName_{0}", id);
					if (formCollection.AllKeys.Contains(ClientDefinedRuleBusinessEntityName) && !string.IsNullOrEmpty(formCollection[ClientDefinedRuleBusinessEntityName]))
					{
						clientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityName = formCollection[ClientDefinedRuleBusinessEntityName];
					}
					
					string ClientDefinedRuleRelationalOperatorId = string.Format("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperatorId_{0}", id);
					if (formCollection.AllKeys.Contains(ClientDefinedRuleRelationalOperatorId) && !string.IsNullOrEmpty(formCollection[ClientDefinedRuleRelationalOperatorId]))
					{
						clientDefinedRuleLogicItem.ClientDefinedRuleRelationalOperatorId = int.Parse(formCollection[ClientDefinedRuleRelationalOperatorId]);
					}

					string ClientDefinedRuleLogicItemValue = string.Format("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue_{0}", id);
					if (formCollection.AllKeys.Contains(ClientDefinedRuleLogicItemValue) && !string.IsNullOrEmpty(formCollection[ClientDefinedRuleLogicItemValue]))
					{
						clientDefinedRuleLogicItem.ClientDefinedRuleLogicItemValue = formCollection[ClientDefinedRuleLogicItemValue];
					}

					clientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem = clientDefinedRuleLogicItem;
					clientDefinedRuleGroupLogicItems.Add(clientDefinedRuleGroupLogic);
				}
			}

			if (clientDefinedRuleGroupLogicItems != null && clientDefinedRuleGroupLogicItems.Count > 0)
			{
				clientDefinedRuleGroupVM.ClientDefinedRuleGroupLogics = clientDefinedRuleGroupLogicItems;
			}

			//Create ClientDefinedRuleResultItems from Post values
			List<ClientDefinedRuleGroupResult> clientDefinedRuleGroupResults = new List<ClientDefinedRuleGroupResult>();
			foreach (string key in formCollection)
			{
				if (key.StartsWith("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityName_") && !string.IsNullOrEmpty(formCollection[key]))
				{
					int id = int.Parse(key.Replace("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityName_", ""));

					ClientDefinedRuleGroupResult clientDefinedRuleGroupResult = new ClientDefinedRuleGroupResult();
					ClientDefinedRuleResultItem clientDefinedRuleResultItem = new ClientDefinedRuleResultItem();

					string ClientDefinedRuleBusinessEntityId = string.Format("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId_{0}", id);
					if (formCollection.AllKeys.Contains(ClientDefinedRuleBusinessEntityId) && !string.IsNullOrEmpty(formCollection[ClientDefinedRuleBusinessEntityId]))
					{
						clientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId = int.Parse(formCollection[ClientDefinedRuleBusinessEntityId]);
					}

					string ClientDefinedRuleBusinessEntityName = string.Format("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityName_{0}", id);
					if (formCollection.AllKeys.Contains(ClientDefinedRuleBusinessEntityName) && !string.IsNullOrEmpty(formCollection[ClientDefinedRuleBusinessEntityName]))
					{
						clientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityName = formCollection[ClientDefinedRuleBusinessEntityName];
					}

					string ClientDefinedRuleResultItemValue = string.Format("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleResultItemValue_{0}", id);
					if (formCollection.AllKeys.Contains(ClientDefinedRuleResultItemValue) && !string.IsNullOrEmpty(formCollection[ClientDefinedRuleResultItemValue]))
					{
						clientDefinedRuleResultItem.ClientDefinedRuleResultItemValue = formCollection[ClientDefinedRuleResultItemValue];
					}

					clientDefinedRuleGroupResult.ClientDefinedRuleResultItem = clientDefinedRuleResultItem;
					clientDefinedRuleGroupResults.Add(clientDefinedRuleGroupResult);
				}
			}

			if (clientDefinedRuleGroupResults != null && clientDefinedRuleGroupResults.Count > 0)
			{
				clientDefinedRuleGroupVM.ClientDefinedRuleGroupResults = clientDefinedRuleGroupResults;
			}

			//Create ClientDefinedRuleWorkflowTriggers from Post values
			List<ClientDefinedRuleGroupTrigger> clientDefinedRuleGroupTriggers = new List<ClientDefinedRuleGroupTrigger>();
			foreach (string key in formCollection)
			{
				if (key.StartsWith("ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId_") && !string.IsNullOrEmpty(formCollection[key]))
				{
					int id = int.Parse(key.Replace("ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId_", ""));

					ClientDefinedRuleGroupTrigger clientDefinedRuleGroupTrigger = new ClientDefinedRuleGroupTrigger();
					ClientDefinedRuleWorkflowTrigger clientDefinedRuleWorkflowTrigger = new ClientDefinedRuleWorkflowTrigger();

					string ClientDefinedRuleWorkflowTriggerStateIdKey = string.Format("ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId_{0}", id);
					if (formCollection.AllKeys.Contains(ClientDefinedRuleWorkflowTriggerStateIdKey) && !string.IsNullOrEmpty(formCollection[ClientDefinedRuleWorkflowTriggerStateIdKey]))
					{
						clientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerStateId = int.Parse(formCollection[ClientDefinedRuleWorkflowTriggerStateIdKey]);
					}

					string ClientDefinedRuleWorkflowTriggerApplicationModeId = string.Format("ClientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationModeId_{0}", id);
					if (formCollection.AllKeys.Contains(ClientDefinedRuleWorkflowTriggerApplicationModeId) && !string.IsNullOrEmpty(formCollection[ClientDefinedRuleWorkflowTriggerApplicationModeId]))
					{
						clientDefinedRuleWorkflowTrigger.ClientDefinedRuleWorkflowTriggerApplicationModeId = int.Parse(formCollection[ClientDefinedRuleWorkflowTriggerApplicationModeId]);
					};

					clientDefinedRuleGroupTrigger.ClientDefinedRuleWorkflowTrigger = clientDefinedRuleWorkflowTrigger;
					clientDefinedRuleGroupTriggers.Add(clientDefinedRuleGroupTrigger);
				}
			}

			if (clientDefinedRuleGroupTriggers != null && clientDefinedRuleGroupTriggers.Count > 0)
			{
				clientDefinedRuleGroupVM.ClientDefinedRuleGroupTriggers = clientDefinedRuleGroupTriggers;
			}

			return clientDefinedRuleGroupVM;
		}

		// POST: /UnDelete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UnDelete(ClientDefinedRuleGroupVM clientDefinedRuleGroupVM)
		{
			//Check Valid Item passed in Form       
			if (clientDefinedRuleGroupVM.ClientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ClientDefinedRuleGroupId);

			//Check Exists in Databsase
			if (clientDefinedRuleGroup == null || clientDefinedRuleGroup.DeletedFlag == false)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}
			
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientDefinedRuleGroup(clientDefinedRuleGroup.ClientDefinedRuleGroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			
			//Delete Form Item
			try
			{
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.DeletedFlag = false;
				clientDefinedRuleGroupRepository.UpdateGroupDeletedStatus(clientDefinedRuleGroupVM.ClientDefinedRuleGroup);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientDefinedRuleGroup.mvc/UnDelete/" + clientDefinedRuleGroup.ClientDefinedRuleGroupId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("ListUnDeleted");
		}

		// GET: /EditSequence for Searched Items
		public ActionResult EditSequence(
			string clientTopUnitName,
			string clientSubUnitName,
			string clientAccountNumber,
			string sourceSystemCode,
			string travelerTypeName,
			string clientDefinedRuleGroupName,
			string filter,
			string sortField,
			int? sortOrder,
			int? page)
		{

			//SortField
			if (string.IsNullOrEmpty(sortField))
			{
				sortField = "GroupSequenceNumber";
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
			
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			var items = clientDefinedRuleGroupRepository.PageClientDefinedRuleGroupSequences(
				clientTopUnitName,
				clientSubUnitName,
				clientAccountNumber,
				sourceSystemCode,
				travelerTypeName,
				clientDefinedRuleGroupName,
				filter,
				sortField,
				sortOrder,
				page ?? 1
			);

			ViewData["Page"] = page ?? 1;

			//Breadcrumb Text
			string searchTerm = "";
			switch (filter)
			{
				case "Client TopUnit Name":
					searchTerm = clientTopUnitName;
					break;
				case "Client SubUnit Name":
					searchTerm = clientSubUnitName;
					break;
				case "Client Account":
					searchTerm = clientAccountNumber + " - " + sourceSystemCode;
					break;
				case "Client Traveler Type":
					searchTerm = clientSubUnitName + " -  " + travelerTypeName;
					break;
				case "Client SubUnit Traveler Type":
					searchTerm = clientSubUnitName + " - " + travelerTypeName;
					break;
				case "Business Group Name":
					searchTerm = clientDefinedRuleGroupName;
					break;
			}

			ViewData["ClientTopUnitName"] = clientTopUnitName ?? "";
			ViewData["ClientSubUnitName"] = clientSubUnitName ?? "";
			ViewData["ClientAccountNumber"] = clientAccountNumber ?? "";
			ViewData["SourceSystemCode"] = sourceSystemCode ?? "";
			ViewData["TravelerTypeName"] = travelerTypeName ?? "";
			ViewData["ClientDefinedRuleGroupName"] = clientDefinedRuleGroupName ?? "";
			ViewData["Filter"] = searchTerm ?? "";

			return View(items);
		}

		// POST: /EditSequence
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult EditSequence(
			string clientTopUnitName,
			string clientSubUnitName,
			string clientAccountNumber,
			string sourceSystemCode,
			string travelerTypeName,
			string clientDefinedRuleGroupName,
			string filter, 
			int page, 
			FormCollection collection)
		{
			//Check Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			string[] sequences = collection["Sequence"].Split(new char[] { ',' });

			int sequence = (page - 1 * 5) - 2;
			if (sequence < 0)
			{
				sequence = 1;
			}

			XmlDocument doc = new XmlDocument();// Create the XML Declaration, and append it to XML document
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("SequenceXML");
			doc.AppendChild(root);

			foreach (string s in sequences)
			{
				string[] primaryKey = s.Split(new char[] { '_' });

				int clientDefinedRuleGroupId = Convert.ToInt32(primaryKey[0]);
				string hierarchyType = primaryKey[1] != null ? primaryKey[1] : "";
				int versionNumber = Convert.ToInt32(primaryKey[2]);

				XmlElement xmlItem = doc.CreateElement("Item");
				root.AppendChild(xmlItem);

				XmlElement xmlGroupSequenceNumber = doc.CreateElement("GroupSequenceNumber");
				xmlGroupSequenceNumber.InnerText = sequence.ToString();
				xmlItem.AppendChild(xmlGroupSequenceNumber);

				XmlElement xmlClientDefinedRuleGroupId = doc.CreateElement("ClientDefinedRuleGroupId");
				xmlClientDefinedRuleGroupId.InnerText = clientDefinedRuleGroupId.ToString();
				xmlItem.AppendChild(xmlClientDefinedRuleGroupId);

				XmlElement xmlHierarchyType = doc.CreateElement("HierarchyType");
				xmlHierarchyType.InnerText = hierarchyType.ToString();
				xmlItem.AppendChild(xmlHierarchyType);

				XmlElement xmlVersionNumber = doc.CreateElement("VersionNumber");
				xmlVersionNumber.InnerText = versionNumber.ToString();
				xmlItem.AppendChild(xmlVersionNumber);

				sequence = sequence + 1;
			}

			try
			{
				clientDefinedRuleGroupRepository.UpdateClientDefinedRuleGroupSequences(System.Xml.Linq.XElement.Parse(doc.OuterXml));
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					string querystring = String.Empty;
					switch (filter)
					{
						case "Client TopUnit Name":
							querystring = "clientTopUnitName=" + clientTopUnitName;
							break;
						case "Client SubUnit Name":
							querystring = "clientSubUnitName=" + clientSubUnitName;
							break;
						case "Client Account":
							querystring = "clientAccountNumber=" + clientAccountNumber + "&sourceSystemCode=" + sourceSystemCode;
							break;
						case "Client Traveler Type":
							querystring = "clientSubUnitName=" + clientSubUnitName + "&travelerTypeName=" + travelerTypeName;
							break;
						case "Client SubUnit Traveler Type":
							querystring = "clientSubUnitName=" + clientSubUnitName + "&travelerTypeName=" + travelerTypeName;
							break;
						case "Business Group Name":
							querystring = "clientDefinedRuleGroupName=" + clientDefinedRuleGroupName;
							break;
					}
					ViewData["ReturnURL"] = "/ClientDefinedBusinessRuleGroup.mvc/EditSequence?filter=" + filter + "&" + querystring;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("ListUnDeleted");
		}

		// GET: /HierarchySearch
		public ActionResult HierarchySearch(int id, string p, string t, string h, string filterHierarchyCSUSearchText = "", string filterHierarchyTTSearchText = "")
		{
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(id);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "HierarchySearchGet";
				return View("RecordDoesNotExistError");
			}

			string filterHierarchySearchProperty = p;
			string filterHierarchySearchText = t;
			string filterHierarchyType = h;
			
			ClientDefinedRuleGroupHierarchySearchVM hierarchySearchVM = new ClientDefinedRuleGroupHierarchySearchVM();
			hierarchySearchVM.GroupId = id;
			hierarchySearchVM.GroupType = groupName;
			hierarchySearchVM.ClientDefinedRuleGroup = clientDefinedRuleGroup;
			hierarchySearchVM.LinkedHierarchies = clientDefinedRuleGroupRepository.ClientDefinedRuleGroupLinkedHierarchies(id, filterHierarchyType);
			hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
			hierarchySearchVM.FilterHierarchySearchText = filterHierarchySearchText;
			hierarchySearchVM.LinkedHierarchiesTotal = clientDefinedRuleGroupRepository.CountClientDefinedRuleGroupLinkedHierarchies(id); ;

			if (filterHierarchySearchProperty == null)
			{
				hierarchySearchVM.AvailableHierarchies = null;
			}
			else
			{
				hierarchySearchVM.AvailableHierarchies = clientDefinedRuleGroupRepository.ClientDefinedRuleGroupAvailableHierarchies(id, filterHierarchySearchProperty, filterHierarchySearchText, filterHierarchyCSUSearchText, filterHierarchyTTSearchText);
			}

			RolesRepository rolesRepository = new RolesRepository();
			hierarchySearchVM.HasWriteAccess = rolesRepository.HasWriteAccessToClientDefinedRuleGroup(id);

			hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
			hierarchySearchVM.HierarchyPropertyOptions = clientDefinedRuleGroupRepository.GetHierarchyPropertyOptions(groupName, clientDefinedRuleGroup.IsBusinessGroupFlag, hierarchySearchVM.FilterHierarchySearchProperty);
			

			return View(hierarchySearchVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult HierarchySearch(int groupId, string filterHierarchySearchProperty, string filterHierarchySearchText, string filterHierarchyCSUSearchText = "", string filterHierarchyTTSearchText = "")
		{

			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedRuleGroupRepository.GetGroup(groupId);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "HierarchySearchGet";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedRuleGroupHierarchySearchVM hierarchySearchVM = new ClientDefinedRuleGroupHierarchySearchVM();
			hierarchySearchVM.GroupId = groupId;
			hierarchySearchVM.GroupType = groupName;
			hierarchySearchVM.ClientDefinedRuleGroup = clientDefinedRuleGroup;
			hierarchySearchVM.LinkedHierarchies = clientDefinedRuleGroupRepository.ClientDefinedRuleGroupLinkedHierarchies(groupId, clientDefinedRuleGroupRepository.getHierarchyType(filterHierarchySearchProperty));
			hierarchySearchVM.AvailableHierarchies = clientDefinedRuleGroupRepository.ClientDefinedRuleGroupAvailableHierarchies(groupId, filterHierarchySearchProperty, filterHierarchySearchText, filterHierarchyCSUSearchText, filterHierarchyTTSearchText);
			hierarchySearchVM.AvailableHierarchyTypeDisplayName = clientDefinedRuleGroupRepository.getAvailableHierarchyTypeDisplayName(filterHierarchySearchProperty);

			if (filterHierarchySearchProperty == null)
			{
				filterHierarchySearchProperty = "ClientSubUnitName";
			}
			hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
			hierarchySearchVM.FilterHierarchySearchText = filterHierarchySearchText;
			hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;

			RolesRepository rolesRepository = new RolesRepository();
			hierarchySearchVM.HasWriteAccess = rolesRepository.HasWriteAccessToClientDefinedRuleGroup(groupId);

			hierarchySearchVM.HierarchyPropertyOptions = clientDefinedRuleGroupRepository.GetHierarchyPropertyOptions(groupName, clientDefinedRuleGroup.IsBusinessGroupFlag, hierarchySearchVM.FilterHierarchySearchProperty);
			hierarchySearchVM.LinkedHierarchiesTotal = clientDefinedRuleGroupRepository.CountClientDefinedRuleGroupLinkedHierarchies(groupId);

			return View(hierarchySearchVM);
		}

		// POST: /AddRemoveHierarchy
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddRemoveHierarchy(ClientDefinedRuleGroupHierarchyVM groupHierarchyVM)
		{
			//Get Item From Database
			ClientDefinedRuleGroup group = new ClientDefinedRuleGroup();
			group = clientDefinedRuleGroupRepository.GetGroup(groupHierarchyVM.GroupId);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "AddRemoveHierarchyPost";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToClientDefinedRuleGroup(groupHierarchyVM.GroupId))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			//Delete Item
			try
			{
				clientDefinedRuleGroupRepository.UpdateLinkedHierarchy(groupHierarchyVM);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/ClientDefinedRuleGroup.mvc/HierarchySearch/" + group.ClientDefinedRuleGroupId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("HierarchySearch", new
			{
				id = group.ClientDefinedRuleGroupId,
				p = groupHierarchyVM.FilterHierarchySearchProperty,
				t = groupHierarchyVM.FilterHierarchySearchText,
				h = clientDefinedRuleGroupRepository.getHierarchyType(groupHierarchyVM.FilterHierarchySearchProperty),
				filterHierarchyCSUSearchText = groupHierarchyVM.FilterHierarchyCSUSearchText,
				filterHierarchyTTSearchText = groupHierarchyVM.FilterHierarchyTTSearchText
			});
		}

		[HttpPost]
		public JsonResult AutoCompleteClientSubUnitTravelerTypes(string searchText, string hierarchyItem, string filterText)
		{
			HierarchyRepository hierarchyRepository = new HierarchyRepository();

			int maxResults = 15;
			var result = hierarchyRepository.LookUpSystemUserClientSubUnitTravelerTypes(searchText, hierarchyItem, maxResults, groupName, filterText);
			return Json(result);
		}

		[HttpPost]
		public JsonResult AutoCompleteClientDefinedRuleLogicBusinessEntities(string searchText)
		{
			ClientDefinedRuleBusinessEntityRepository clientDefinedRuleBusinessEntityRepository = new ClientDefinedRuleBusinessEntityRepository();
			var result = clientDefinedRuleBusinessEntityRepository.AutoCompleteClientDefinedRuleLogicBusinessEntities(searchText);
			return Json(result);
		}
		
		[HttpPost]
		public JsonResult AutoCompleteClientDefinedRuleResultBusinessEntities(string searchText)
		{
			ClientDefinedRuleBusinessEntityRepository clientDefinedRuleBusinessEntityRepository = new ClientDefinedRuleBusinessEntityRepository();
			var result = clientDefinedRuleBusinessEntityRepository.AutoCompleteClientDefinedRuleResultBusinessEntities(searchText);
			return Json(result);
		}
	}
}
