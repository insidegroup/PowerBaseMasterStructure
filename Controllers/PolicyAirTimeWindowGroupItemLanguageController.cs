using ClientProfileServiceBusiness;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace CWTDesktopDatabase.Controllers
{
	public class PolicyAirTimeWindowGroupItemLanguageController : Controller
	{
		// Main repository
		PolicyAirParameterGroupItemRepository policyAirParameterGroupItemRepository = new PolicyAirParameterGroupItemRepository();
		PolicyAirParameterGroupItemLanguageRepository policyAirParameterGroupItemLanguageRepository = new PolicyAirParameterGroupItemLanguageRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		PolicyRoutingRepository policyRoutingRepository = new PolicyRoutingRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /PolicyAirParameterGroupItemLanguage/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyAirParameterGroupItemLanguagesVM policyAirParameterGroupItemLanguagesVM = new PolicyAirParameterGroupItemLanguagesVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				return View("Error");
			}

			policyAirParameterGroupItemLanguagesVM.PolicyGroup = policyGroup;

			//Check PolicyAirParameterGroupItem Exists
			PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
			policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(id);
			if (policyAirParameterGroupItem == null)
			{
				return View("Error");
			}

			policyAirParameterGroupItemLanguagesVM.PolicyAirParameterGroupItem = policyAirParameterGroupItem;

			//Set Access Rights
			policyAirParameterGroupItemLanguagesVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policyAirParameterGroupItemLanguagesVM.HasWriteAccess = true;
			}
			
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

			policyAirParameterGroupItemLanguagesVM.PolicyAirParameterGroupItemLanguages = policyAirParameterGroupItemLanguageRepository.PagePolicyAirParameterGroupItemLanguages(id, policyGroupId, filter ?? "", page ?? 1, sortField, sortOrder ?? 0);

			return View(policyAirParameterGroupItemLanguagesVM);
		}

		// GET: /PolicyAirParameterGroupItemLanguage/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyAirParameterGroupItemLanguageVM policyAirParameterGroupItemLanguageVM = new PolicyAirParameterGroupItemLanguageVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyAirParameterGroupItem Exists
			PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
			policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(id);
			if (policyAirParameterGroupItem == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItem = policyAirParameterGroupItem;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAirParameterGroupItemLanguageVM.PolicyGroup = policyGroup;

			PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage = new PolicyAirParameterGroupItemLanguage();
			policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguage;

			//Languages
			SelectList languages = new SelectList(policyAirParameterGroupItemLanguageRepository.GetAvailableLanguages(id).ToList(), "LanguageCode", "LanguageName");
			policyAirParameterGroupItemLanguageVM.Languages = languages;

			return View(policyAirParameterGroupItemLanguageVM);
		}

		// POST: /PolicyAirParameterGroupItemLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyAirParameterGroupItemLanguageVM policyAirParameterGroupItemLanguageVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyAirParameterGroupItemLanguageVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyAirParameterGroupItem Exists
			PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
			policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItem.PolicyAirParameterGroupItemId);
			if (policyAirParameterGroupItem == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage = new PolicyAirParameterGroupItemLanguage();
			policyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage;
			if (policyAirParameterGroupItemLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			policyAirParameterGroupItemLanguage.PolicyAirParameterGroupItemId = policyAirParameterGroupItem.PolicyAirParameterGroupItemId;

			//Database Update
			try
			{
				policyAirParameterGroupItemLanguageRepository.Add(policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage);
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
			return RedirectToAction("List", new { id = policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItem.PolicyAirParameterGroupItemId, policyGroupId = policyAirParameterGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyAirParameterGroupItemLanguage/Edit
		public ActionResult Edit(int policyGroupId, int policyAirParameterGroupItemId, string languageCode)
		{
			PolicyAirParameterGroupItemLanguageVM policyAirParameterGroupItemLanguageVM = new PolicyAirParameterGroupItemLanguageVM();

			//Check Item Exists
			PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage = new PolicyAirParameterGroupItemLanguage();
			policyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguageRepository.GetItem(
				policyAirParameterGroupItemId,
				languageCode
			);
			if (policyAirParameterGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyAirParameterGroupItem Exists
			PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
			policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(policyAirParameterGroupItemId);
			if (policyAirParameterGroupItem == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Policy Routing
			PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirParameterGroupItem.PolicyRoutingId ?? 0);
			if (policyRouting != null)
			{
				policyRoutingRepository.EditForDisplay(policyRouting);
				policyAirParameterGroupItem.PolicyRouting = policyRouting;
			}

			//Policy Group
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAirParameterGroupItemLanguageVM.PolicyGroup = policyGroup;

			//Languages
			List<Language> availableLanguages = policyAirParameterGroupItemLanguageRepository.GetAvailableLanguages(policyAirParameterGroupItemId).ToList();
			Language selectedLanguage = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			selectedLanguage = languageRepository.GetLanguage(policyAirParameterGroupItemLanguage.LanguageCode);
			if (selectedLanguage != null)
			{
				availableLanguages.Add(selectedLanguage);
			}

			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageCode), "LanguageCode", "LanguageName", policyAirParameterGroupItemLanguage.LanguageCode);
			policyAirParameterGroupItemLanguageVM.Languages = languages;
			
			policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItem = policyAirParameterGroupItem;
			policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguage;


			return View(policyAirParameterGroupItemLanguageVM);
		}

		// POST: /PolicyAirParameterGroupItemLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyAirParameterGroupItemLanguageVM policyAirParameterGroupItemLanguageVM, FormCollection formCollection)
		{
			//Get Item
			PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage = new PolicyAirParameterGroupItemLanguage();
			policyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguageRepository.GetItem(
				policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItem.PolicyAirParameterGroupItemId,
				policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage.LanguageCode
			);

			//Check Exists
			if (policyAirParameterGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Database Update
			try
			{
				policyAirParameterGroupItemLanguageRepository.Update(policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage);
			}
			catch (SqlException ex)
			{
				//Non-Unique Name
				if (ex.Message == "NonUniqueName")
				{
					return View("NonUniqueNameError");
				}
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyAirParameterGroupItemLanguage.mvc/Edit/" + policyAirParameterGroupItemLanguage.PolicyAirParameterGroupItemId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { 
				id = policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItem.PolicyAirParameterGroupItemId, 
				policyGroupId = policyAirParameterGroupItemLanguageVM.PolicyGroup.PolicyGroupId
			});
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int policyGroupId, int policyAirParameterGroupItemId, string languageCode)
		{
			PolicyAirParameterGroupItemLanguageVM policyAirParameterGroupItemLanguageVM = new PolicyAirParameterGroupItemLanguageVM();

			//Check Item Exists
			PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage = new PolicyAirParameterGroupItemLanguage();
			policyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguageRepository.GetItem(
				policyAirParameterGroupItemId,
				languageCode
			);

			if (policyAirParameterGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyAirParameterGroupItem Exists
			PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
			policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(policyAirParameterGroupItemId);
			if (policyAirParameterGroupItem == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Policy Routing
			PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirParameterGroupItem.PolicyRoutingId ?? 0);
			if (policyRouting != null)
			{
				policyRoutingRepository.EditForDisplay(policyRouting);
				policyAirParameterGroupItem.PolicyRouting = policyRouting;
			}

			//Policy Group
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAirParameterGroupItemLanguageVM.PolicyGroup = policyGroup;

			policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItem = policyAirParameterGroupItem;
			policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguage;

			return View(policyAirParameterGroupItemLanguageVM);
		}

		// POST: /PolicyAirParameterGroupItemLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyAirParameterGroupItemLanguageVM policyAirParameterGroupItemLanguageVM)
		{
			//Check Valid Item passed in Form       
			if (policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage = new PolicyAirParameterGroupItemLanguage();
			policyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguageRepository.GetItem(
				policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage.PolicyAirParameterGroupItemId,
				policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage.LanguageCode
			);

			//Check Exists
			if (policyAirParameterGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "DeleteGet";
				return View("RecordDoesNotExistError");
			}
		
			//Delete Form Item
			try
			{
				policyAirParameterGroupItemLanguageRepository.Delete(policyAirParameterGroupItemLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyAirParameterGroupItemLanguage.mvc/Delete/" + policyAirParameterGroupItemLanguage.PolicyAirParameterGroupItemId.ToString() + "/" + policyAirParameterGroupItemLanguage.LanguageCode;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItem.PolicyAirParameterGroupItemId, policyGroupId = policyAirParameterGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /View
		public ActionResult View(int policyGroupId, int policyAirParameterGroupItemId, string languageCode)
		{
			PolicyAirParameterGroupItemLanguageVM policyAirParameterGroupItemLanguageVM = new PolicyAirParameterGroupItemLanguageVM();

			//Check Item Exists
			PolicyAirParameterGroupItemLanguage policyAirParameterGroupItemLanguage = new PolicyAirParameterGroupItemLanguage();
			policyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguageRepository.GetItem(
				policyAirParameterGroupItemId,
				languageCode
			);

			if (policyAirParameterGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyAirParameterGroupItem Exists
			PolicyAirParameterGroupItem policyAirParameterGroupItem = new PolicyAirParameterGroupItem();
			policyAirParameterGroupItem = policyAirParameterGroupItemRepository.GetPolicyAirParameterGroupItem(policyAirParameterGroupItemId);
			if (policyAirParameterGroupItem == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Policy Routing
			PolicyRouting policyRouting = policyRoutingRepository.GetPolicyRouting(policyAirParameterGroupItem.PolicyRoutingId ?? 0);
			if (policyRouting != null)
			{
				policyRoutingRepository.EditForDisplay(policyRouting);
				policyAirParameterGroupItem.PolicyRouting = policyRouting;
			} 
			
			//Policy Group
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAirParameterGroupItemLanguageVM.PolicyGroup = policyGroup;
			
			policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItem = policyAirParameterGroupItem;
			policyAirParameterGroupItemLanguageVM.PolicyAirParameterGroupItemLanguage = policyAirParameterGroupItemLanguage;
			
			return View(policyAirParameterGroupItemLanguageVM);
		}

	}
}
