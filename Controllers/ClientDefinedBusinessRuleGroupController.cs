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

namespace CWTDesktopDatabase.Controllers
{
    public class ClientDefinedBusinessRuleGroupController : Controller
    {
        //main repository
        ClientDefinedBusinessRuleGroupRepository clientDefinedBusinessRuleGroupRepository = new ClientDefinedBusinessRuleGroupRepository();
        HierarchyRepository hierarchyRepository = new HierarchyRepository();

        private string groupName = "Business Rules Group Administrator";

        // GET: /ListUnDeleted
        public ActionResult ListUnDeleted(
            string filter,
            int? page,
            string sortField,
            int? sortOrder,
            string category,
            string clientDefinedRuleGroupName)
        {
            //Filter
            //Odd chars in filter
            if (filter != null)
            {
                if (filter.StartsWith("Category"))
                {
                    filter = "Category";
                }
                else if (filter.StartsWith("Business Rules Group Name"))
                {
                    filter = "Business Rules Group Name";
                }
            }
           
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

			ClientDefinedBusinessRuleGroupsVM clientDefinedBusinessRuleGroupsVM = new ClientDefinedBusinessRuleGroupsVM();

            //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
				clientDefinedBusinessRuleGroupsVM.HasDomainWriteAccess = true;
            }

			if (clientDefinedBusinessRuleGroupRepository != null)
			{
				var clientDefinedBusinessRuleGroups = clientDefinedBusinessRuleGroupRepository.PageClientDefinedBusinessRuleGroups(
					false,
					category, 
					clientDefinedRuleGroupName, 
					page ?? 1, 
					filter ?? "", 
					sortField, 
					sortOrder ?? 0
				);

				if (clientDefinedBusinessRuleGroups != null)
				{
					clientDefinedBusinessRuleGroupsVM.ClientDefinedBusinessRuleGroups = clientDefinedBusinessRuleGroups;
				}
			}

			//Search Fields
			SelectList searchListFilters = new SelectList(clientDefinedBusinessRuleGroupRepository.GetSearchListFilters().ToList(), filter);
			if (searchListFilters != null)
			{
				clientDefinedBusinessRuleGroupsVM.SearchListFilters = searchListFilters;
			}

			//ClientDefinedRuleBusinessEntityCategories
			ClientDefinedRuleBusinessEntityCategoryRepository clientDefinedRuleBusinessEntityCategoryRepository = new ClientDefinedRuleBusinessEntityCategoryRepository();
			SelectList clientDefinedRuleBusinessEntityCategories = 
				new SelectList(
					clientDefinedRuleBusinessEntityCategoryRepository.GetClientDefinedRuleBusinessEntityCategories().ToList(),
					"clientDefinedRuleBusinessEntityCategoryDescription",
					"clientDefinedRuleBusinessEntityCategoryDescription",
					category ?? ""
				);
			if (clientDefinedRuleBusinessEntityCategories != null)
			{
				clientDefinedBusinessRuleGroupsVM.ClientDefinedRuleBusinessEntityCategories = clientDefinedRuleBusinessEntityCategories;
			}
						
			clientDefinedBusinessRuleGroupsVM.Filter = filter ?? "";
			clientDefinedBusinessRuleGroupsVM.Category = category ?? "";
			clientDefinedBusinessRuleGroupsVM.ClientDefinedRuleGroupName = clientDefinedRuleGroupName ?? "";

			//Breadcrumb Text
			string searchTerm = "";
			switch (filter)
			{
				case "Category":
					searchTerm = category;
					break;
				case "Business Rules Group Name":
					searchTerm = clientDefinedRuleGroupName;
					break;
			}
			clientDefinedBusinessRuleGroupsVM.SearchTerm = searchTerm;

			//return items
			return View(clientDefinedBusinessRuleGroupsVM);     
        }

		// GET: /ListDeleted
		public ActionResult ListDeleted(
			string filter, 
			int? page, 
			string sortField, 
			int? sortOrder,
			string category, 
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

			ClientDefinedBusinessRuleGroupsVM clientDefinedBusinessRuleGroupsVM = new ClientDefinedBusinessRuleGroupsVM();

            //Set Access Rights
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                clientDefinedBusinessRuleGroupsVM.HasDomainWriteAccess = true;
            }

			if (clientDefinedBusinessRuleGroupRepository != null)
			{
				var clientDefinedBusinessRuleGroups = clientDefinedBusinessRuleGroupRepository.PageClientDefinedBusinessRuleGroups(
					true,
					category, 
					clientDefinedRuleGroupName, 
					page ?? 1, 
					filter ?? "", 
					sortField, 
					sortOrder ?? 0
				);

				if (clientDefinedBusinessRuleGroups != null)
				{
					clientDefinedBusinessRuleGroupsVM.ClientDefinedBusinessRuleGroups = clientDefinedBusinessRuleGroups;
				}
			}

			//Search Fields
			SelectList searchListFilters = new SelectList(clientDefinedBusinessRuleGroupRepository.GetSearchListFilters().ToList(), filter);
			clientDefinedBusinessRuleGroupsVM.SearchListFilters = searchListFilters;
			clientDefinedBusinessRuleGroupsVM.Filter = filter ?? "";
			clientDefinedBusinessRuleGroupsVM.Category = category ?? "";
			clientDefinedBusinessRuleGroupsVM.ClientDefinedRuleGroupName = clientDefinedRuleGroupName ?? "";

			//return items
			return View(clientDefinedBusinessRuleGroupsVM);     
		}
		
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
				clientDefinedBusinessRuleGroupRepository.Add(clientDefinedRuleGroupVM);
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
			clientDefinedRuleGroup = clientDefinedBusinessRuleGroupRepository.GetGroup(id);

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
			clientDefinedBusinessRuleGroupRepository.EditGroupForDisplay(clientDefinedRuleGroup);
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
			clientDefinedRuleGroup = clientDefinedBusinessRuleGroupRepository.GetGroup(clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ClientDefinedRuleGroupId);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
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

			//Database Update
			try
			{
				clientDefinedBusinessRuleGroupRepository.Edit(clientDefinedRuleGroupVM);
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
			clientDefinedRuleGroup = clientDefinedBusinessRuleGroupRepository.GetGroup(id);

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
			clientDefinedRuleGroup = clientDefinedBusinessRuleGroupRepository.GetGroup(id);

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
			clientDefinedRuleGroup = clientDefinedBusinessRuleGroupRepository.GetGroup(clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ClientDefinedRuleGroupId);

			//Check Exists in Databsase
			if (clientDefinedRuleGroup == null || clientDefinedRuleGroup.DeletedFlag == true)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}
			//Check Access
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Form Item
			try
			{
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.DeletedFlag = true;
				clientDefinedBusinessRuleGroupRepository.UpdateGroupDeletedStatus(clientDefinedRuleGroupVM.ClientDefinedRuleGroup);
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
			clientDefinedRuleGroup = clientDefinedBusinessRuleGroupRepository.GetGroup(id);

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
			clientDefinedBusinessRuleGroupRepository.EditGroupForDisplay(clientDefinedRuleGroup);
			clientDefinedRuleGroupVM.ClientDefinedRuleGroup = clientDefinedRuleGroup;

			//ClientDefinedRuleBusinessEntities
			ClientDefinedRuleBusinessEntityRepository clientDefinedRuleBusinessEntityRepository = new ClientDefinedRuleBusinessEntityRepository();
			SelectList clientDefinedRuleBusinessEntities = new SelectList(clientDefinedRuleBusinessEntityRepository.GetClientDefinedRuleResultBusinessEntities().ToList(), "ClientDefinedRuleBusinessEntityId", "BusinessEntityDescription");
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
				if (key.StartsWith("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId_") && !string.IsNullOrEmpty(formCollection[key]))
				{
					int id = int.Parse(key.Replace("ClientDefinedRuleGroupLogic.ClientDefinedRuleLogicItem.ClientDefinedRuleBusinessEntityId_", ""));

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
				if (key.StartsWith("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId_") && !string.IsNullOrEmpty(formCollection[key]))
				{
					int id = int.Parse(key.Replace("ClientDefinedRuleGroupResult.ClientDefinedRuleResultItem.ClientDefinedRuleBusinessEntityId_", ""));

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
			clientDefinedRuleGroup = clientDefinedBusinessRuleGroupRepository.GetGroup(clientDefinedRuleGroupVM.ClientDefinedRuleGroup.ClientDefinedRuleGroupId);

			//Check Exists in Databsase
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			
			//Delete Form Item
			try
			{
				clientDefinedRuleGroupVM.ClientDefinedRuleGroup.DeletedFlag = false;
				clientDefinedBusinessRuleGroupRepository.UpdateGroupDeletedStatus(clientDefinedRuleGroupVM.ClientDefinedRuleGroup);
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

		// GET: /HierarchySearch
		public ActionResult HierarchySearch(int id, string p, string t, string h, string filterHierarchyCSUSearchText = "", string filterHierarchyTTSearchText = "")
		{
			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedBusinessRuleGroupRepository.GetGroup(id);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "HierarchySearchGet";
				return View("RecordDoesNotExistError");
			}

			string filterHierarchySearchProperty = p;
			string filterHierarchySearchText = t;
			string filterHierarchyType = h;

			ClientDefinedBusinessRuleGroupHierarchySearchVM hierarchySearchVM = new ClientDefinedBusinessRuleGroupHierarchySearchVM();
			hierarchySearchVM.GroupId = id;
			hierarchySearchVM.GroupType = groupName;
			hierarchySearchVM.ClientDefinedRuleGroup = clientDefinedRuleGroup;
			hierarchySearchVM.LinkedHierarchies = clientDefinedBusinessRuleGroupRepository.ClientDefinedBusinessRuleGroupLinkedHierarchies(id, filterHierarchyType);
			hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
			hierarchySearchVM.FilterHierarchySearchText = filterHierarchySearchText;
			hierarchySearchVM.LinkedHierarchiesTotal = clientDefinedBusinessRuleGroupRepository.CountClientDefinedRuleGroupLinkedHierarchies(id); ;
			hierarchySearchVM.AvailableHierarchyTypeDisplayName = clientDefinedBusinessRuleGroupRepository.GetAvailableHierarchyTypeDisplayName(filterHierarchySearchProperty);

			if (filterHierarchySearchProperty == null)
			{
				hierarchySearchVM.AvailableHierarchies = null;
			}
			else
			{
				hierarchySearchVM.AvailableHierarchies = clientDefinedBusinessRuleGroupRepository.ClientDefinedBusinessRuleGroupAvailableHierarchies(id, filterHierarchySearchProperty, filterHierarchySearchText, filterHierarchyCSUSearchText, filterHierarchyTTSearchText);
			}
			hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
			
			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchiesForHierarchySearch(groupName).ToList(), "Value", "Text", hierarchySearchVM.FilterHierarchySearchProperty);
			if (hierarchyTypesList != null)
			{
				hierarchySearchVM.HierarchyPropertyOptions = hierarchyTypesList;
			}

			RolesRepository rolesRepository = new RolesRepository();
			hierarchySearchVM.HasWriteAccess = rolesRepository.HasWriteAccessToClientDefinedRuleGroup(id);

			return View(hierarchySearchVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult HierarchySearch(int groupId, string filterHierarchySearchText, string filterHierarchySearchProperty, string filterHierarchyCSUSearchText = "", string filterHierarchyTTSearchText = "")
		{

			ClientDefinedRuleGroup clientDefinedRuleGroup = new ClientDefinedRuleGroup();
			clientDefinedRuleGroup = clientDefinedBusinessRuleGroupRepository.GetGroup(groupId);

			//Check Exists
			if (clientDefinedRuleGroup == null)
			{
				ViewData["ActionMethod"] = "HierarchySearchGet";
				return View("RecordDoesNotExistError");
			}

			ClientDefinedBusinessRuleGroupHierarchySearchVM hierarchySearchVM = new ClientDefinedBusinessRuleGroupHierarchySearchVM();
			hierarchySearchVM.GroupId = groupId;
			hierarchySearchVM.GroupType = groupName;
			hierarchySearchVM.ClientDefinedRuleGroup = clientDefinedRuleGroup;
			hierarchySearchVM.LinkedHierarchies = clientDefinedBusinessRuleGroupRepository.ClientDefinedBusinessRuleGroupLinkedHierarchies(groupId, clientDefinedBusinessRuleGroupRepository.getHierarchyType(filterHierarchySearchProperty));
			hierarchySearchVM.AvailableHierarchies = clientDefinedBusinessRuleGroupRepository.ClientDefinedBusinessRuleGroupAvailableHierarchies(groupId, filterHierarchySearchProperty, filterHierarchySearchText, filterHierarchyCSUSearchText, filterHierarchyTTSearchText);
			hierarchySearchVM.LinkedHierarchiesTotal = clientDefinedBusinessRuleGroupRepository.CountClientDefinedRuleGroupLinkedHierarchies(groupId);
			hierarchySearchVM.AvailableHierarchyTypeDisplayName = clientDefinedBusinessRuleGroupRepository.GetAvailableHierarchyTypeDisplayName(filterHierarchySearchProperty);

			if (filterHierarchySearchProperty == null)
			{
				filterHierarchySearchProperty = "ClientSubUnitName";
			}
			hierarchySearchVM.FilterHierarchySearchProperty = filterHierarchySearchProperty;
			hierarchySearchVM.FilterHierarchySearchText = filterHierarchySearchText;
			TablesDomainHierarchyLevelRepository tablesDomainHierarchyLevelRepository = new TablesDomainHierarchyLevelRepository();
			SelectList hierarchyTypesList = new SelectList(tablesDomainHierarchyLevelRepository.GetDomainHierarchiesForHierarchySearch(groupName).ToList(), "Value", "Text", hierarchySearchVM.FilterHierarchySearchProperty);

			hierarchySearchVM.HierarchyPropertyOptions = hierarchyTypesList;

			RolesRepository rolesRepository = new RolesRepository();
			hierarchySearchVM.HasWriteAccess = rolesRepository.HasWriteAccessToClientDefinedRuleGroup(groupId);

			return View(hierarchySearchVM);
		}

		// POST: /AddRemoveHierarchy
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult AddRemoveHierarchy(GroupHierarchyVM groupHierarchyVM)
		{
			//Get Item From Database
			ClientDefinedRuleGroup group = new ClientDefinedRuleGroup();
			group = clientDefinedBusinessRuleGroupRepository.GetGroup(groupHierarchyVM.GroupId);

			//Check Exists
			if (group == null)
			{
				ViewData["ActionMethod"] = "AddRemoveHierarchyPost";
				return View("RecordDoesNotExistError");
			}
			
			//Check Access
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}
			//Delete Item
			try
			{
				clientDefinedBusinessRuleGroupRepository.UpdateLinkedHierarchy(groupHierarchyVM);
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
			return RedirectToAction("HierarchySearch", new { 
				id = group.ClientDefinedRuleGroupId, 
				p = groupHierarchyVM.FilterHierarchySearchProperty, 
				t = groupHierarchyVM.FilterHierarchySearchText, 
				h = clientDefinedBusinessRuleGroupRepository.getHierarchyType(groupHierarchyVM.FilterHierarchySearchProperty),
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
    }
}
