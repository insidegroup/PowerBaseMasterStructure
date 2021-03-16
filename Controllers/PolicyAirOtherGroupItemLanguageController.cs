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
	public class PolicyAirOtherGroupItemLanguageController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyAirOtherGroupItemLanguageRepository policyAirOtherGroupItemLanguageRepository = new PolicyAirOtherGroupItemLanguageRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /PolicyAirOtherGroupItemLanguage/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyAirOtherGroupItemLanguagesVM policyAirOtherGroupItemLanguagesVM = new PolicyAirOtherGroupItemLanguagesVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				return View("Error");
			}

			policyAirOtherGroupItemLanguagesVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				return View("Error");
			}

			policyAirOtherGroupItemLanguagesVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policyAirOtherGroupItemLanguagesVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policyAirOtherGroupItemLanguagesVM.HasWriteAccess = true;
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

			policyAirOtherGroupItemLanguagesVM.PolicyAirVendorGroupItemLanguages = policyAirOtherGroupItemLanguageRepository.GetPolicyAirOtherGroupItemLanguages(id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);

			return View(policyAirOtherGroupItemLanguagesVM);
		}

		// GET: /PolicyAirOtherGroupItemLanguage/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyAirOtherGroupItemLanguageVM policyAirOtherGroupItemLanguageVM = new PolicyAirOtherGroupItemLanguageVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}

			policyAirOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAirOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			PolicyAirOtherGroupItemLanguage policyAirOtherGroupItemLanguage = new PolicyAirOtherGroupItemLanguage();
			policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage = policyAirOtherGroupItemLanguage;

			//Languages
			SelectList languages = new SelectList(policyAirOtherGroupItemLanguageRepository.GetAvailableLanguages(id, policyGroupId).ToList(), "LanguageCode", "LanguageName");
			policyAirOtherGroupItemLanguageVM.Languages = languages;

			return View(policyAirOtherGroupItemLanguageVM);
		}

		// POST: /PolicyAirOtherGroupItemLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyAirOtherGroupItemLanguageVM policyAirOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyAirOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyAirOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			PolicyAirOtherGroupItemLanguage policyAirOtherGroupItemLanguage = new PolicyAirOtherGroupItemLanguage();
			policyAirOtherGroupItemLanguage = policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage;
			if (policyAirOtherGroupItemLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyAirOtherGroupItemLanguage>(policyAirOtherGroupItemLanguage, "PolicyAirOtherGroupItemLanguage");
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
				policyAirOtherGroupItemLanguageRepository.Add(policyAirOtherGroupItemLanguageVM);
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
			return RedirectToAction("List", new { id = policyAirOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAirOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyAirOtherGroupItemLanguage/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyAirOtherGroupItemLanguageVM policyAirOtherGroupItemLanguageVM = new PolicyAirOtherGroupItemLanguageVM();

			//Check Item Exists
			PolicyAirOtherGroupItemLanguage policyAirOtherGroupItemLanguage = new PolicyAirOtherGroupItemLanguage();
			policyAirOtherGroupItemLanguage = policyAirOtherGroupItemLanguageRepository.GetPolicyAirOtherGroupItemLanguage(id);
			if (policyAirOtherGroupItemLanguage == null)
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

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}
	
			policyAirOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage = policyAirOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAirOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			//Languages
			List<Language> availableLanguages = policyAirOtherGroupItemLanguageRepository.GetAvailableLanguages(policyOtherGroupHeaderId, policyGroup.PolicyGroupId).ToList();
			Language selectedLanguage = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			selectedLanguage = languageRepository.GetLanguage(policyAirOtherGroupItemLanguage.LanguageCode);
			if (selectedLanguage != null)
			{
				availableLanguages.Add(selectedLanguage);
			}

			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageCode), "LanguageCode", "LanguageName", policyAirOtherGroupItemLanguage.LanguageCode);
			policyAirOtherGroupItemLanguageVM.Languages = languages;

			return View(policyAirOtherGroupItemLanguageVM);
		}

		// POST: /PolicyAirOtherGroupItemLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyAirOtherGroupItemLanguageVM policyAirOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Get Item
			PolicyAirOtherGroupItemLanguage policyAirOtherGroupItemLanguage = new PolicyAirOtherGroupItemLanguage();
			policyAirOtherGroupItemLanguage = policyAirOtherGroupItemLanguageRepository.GetPolicyAirOtherGroupItemLanguage(policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage.PolicyAirOtherGroupItemLanguageId);

			//Check Exists
			if (policyAirOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyAirOtherGroupItemLanguage>(policyAirOtherGroupItemLanguage, "PolicyAirOtherGroupItemLanguage");
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
				policyAirOtherGroupItemLanguageRepository.Edit(policyAirOtherGroupItemLanguageVM);
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
					ViewData["ReturnURL"] = "/PolicyAirOtherGroupItemLanguage.mvc/Edit/" + policyAirOtherGroupItemLanguage.PolicyAirOtherGroupItemLanguageId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = policyAirOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAirOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyAirOtherGroupItemLanguageVM policyAirOtherGroupItemLanguageVM = new PolicyAirOtherGroupItemLanguageVM();

			//Check Item Exists
			PolicyAirOtherGroupItemLanguage policyAirOtherGroupItemLanguage = new PolicyAirOtherGroupItemLanguage();
			policyAirOtherGroupItemLanguage = policyAirOtherGroupItemLanguageRepository.GetPolicyAirOtherGroupItemLanguage(id);
			if (policyAirOtherGroupItemLanguage == null)
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

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreateGet";
				return View("RecordDoesNotExistError");
			}
	
			policyAirOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage = policyAirOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAirOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			return View(policyAirOtherGroupItemLanguageVM);
		}

		// POST: /PolicyAirOtherGroupItemLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyAirOtherGroupItemLanguageVM policyAirOtherGroupItemLanguageVM)
		{
			//Check Valid Item passed in Form       
			if (policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyAirOtherGroupItemLanguage policyAirOtherGroupItemLanguage = new PolicyAirOtherGroupItemLanguage();
			policyAirOtherGroupItemLanguage = policyAirOtherGroupItemLanguageRepository.GetPolicyAirOtherGroupItemLanguage(policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage.PolicyAirOtherGroupItemLanguageId);

			//Check Exists
			if (policyAirOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
		
			//Delete Form Item
			try
			{
				policyAirOtherGroupItemLanguageRepository.Delete(policyAirOtherGroupItemLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyAirOtherGroupItemLanguage.mvc/Delete/" + policyAirOtherGroupItemLanguageVM.PolicyAirOtherGroupItemLanguage.PolicyAirOtherGroupItemLanguageId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyAirOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAirOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}
	}
}
