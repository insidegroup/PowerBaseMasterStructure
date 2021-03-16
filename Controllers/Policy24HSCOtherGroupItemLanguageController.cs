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
	public class Policy24HSCOtherGroupItemLanguageController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		Policy24HSCOtherGroupItemLanguageRepository policy24HSCOtherGroupItemLanguageRepository = new Policy24HSCOtherGroupItemLanguageRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /Policy24HSCOtherGroupItemLanguage/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			Policy24HSCOtherGroupItemLanguagesVM policy24HSCOtherGroupItemLanguagesVM = new Policy24HSCOtherGroupItemLanguagesVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				return View("Error");
			}

			policy24HSCOtherGroupItemLanguagesVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				return View("Error");
			}

			policy24HSCOtherGroupItemLanguagesVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policy24HSCOtherGroupItemLanguagesVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policy24HSCOtherGroupItemLanguagesVM.HasWriteAccess = true;
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

			policy24HSCOtherGroupItemLanguagesVM.Policy24HSCVendorGroupItemLanguages = policy24HSCOtherGroupItemLanguageRepository.GetPolicy24HSCOtherGroupItemLanguages(id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);

			return View(policy24HSCOtherGroupItemLanguagesVM);
		}

		// GET: /Policy24HSCOtherGroupItemLanguage/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			Policy24HSCOtherGroupItemLanguageVM policy24HSCOtherGroupItemLanguageVM = new Policy24HSCOtherGroupItemLanguageVM();

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

			policy24HSCOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policy24HSCOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			Policy24HSCOtherGroupItemLanguage policy24HSCOtherGroupItemLanguage = new Policy24HSCOtherGroupItemLanguage();
			policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage = policy24HSCOtherGroupItemLanguage;

			//Languages
			SelectList languages = new SelectList(policy24HSCOtherGroupItemLanguageRepository.GetAvailableLanguages(id, policyGroupId).ToList(), "LanguageCode", "LanguageName");
			policy24HSCOtherGroupItemLanguageVM.Languages = languages;

			return View(policy24HSCOtherGroupItemLanguageVM);
		}

		// POST: /Policy24HSCOtherGroupItemLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Policy24HSCOtherGroupItemLanguageVM policy24HSCOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policy24HSCOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policy24HSCOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			Policy24HSCOtherGroupItemLanguage policy24HSCOtherGroupItemLanguage = new Policy24HSCOtherGroupItemLanguage();
			policy24HSCOtherGroupItemLanguage = policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage;
			if (policy24HSCOtherGroupItemLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<Policy24HSCOtherGroupItemLanguage>(policy24HSCOtherGroupItemLanguage, "Policy24HSCOtherGroupItemLanguage");
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
				policy24HSCOtherGroupItemLanguageRepository.Add(policy24HSCOtherGroupItemLanguageVM);
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
			return RedirectToAction("List", new { id = policy24HSCOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policy24HSCOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Policy24HSCOtherGroupItemLanguage/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			Policy24HSCOtherGroupItemLanguageVM policy24HSCOtherGroupItemLanguageVM = new Policy24HSCOtherGroupItemLanguageVM();

			//Check Item Exists
			Policy24HSCOtherGroupItemLanguage policy24HSCOtherGroupItemLanguage = new Policy24HSCOtherGroupItemLanguage();
			policy24HSCOtherGroupItemLanguage = policy24HSCOtherGroupItemLanguageRepository.GetPolicy24HSCOtherGroupItemLanguage(id);
			if (policy24HSCOtherGroupItemLanguage == null)
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
	
			policy24HSCOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage = policy24HSCOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policy24HSCOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			//Languages
			List<Language> availableLanguages = policy24HSCOtherGroupItemLanguageRepository.GetAvailableLanguages(policyOtherGroupHeaderId, policyGroup.PolicyGroupId).ToList();
			Language selectedLanguage = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			selectedLanguage = languageRepository.GetLanguage(policy24HSCOtherGroupItemLanguage.LanguageCode);
			if (selectedLanguage != null)
			{
				availableLanguages.Add(selectedLanguage);
			}

			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageCode), "LanguageCode", "LanguageName", policy24HSCOtherGroupItemLanguage.LanguageCode);
			policy24HSCOtherGroupItemLanguageVM.Languages = languages;

			return View(policy24HSCOtherGroupItemLanguageVM);
		}

		// POST: /Policy24HSCOtherGroupItemLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Policy24HSCOtherGroupItemLanguageVM policy24HSCOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Get Item
			Policy24HSCOtherGroupItemLanguage policy24HSCOtherGroupItemLanguage = new Policy24HSCOtherGroupItemLanguage();
			policy24HSCOtherGroupItemLanguage = policy24HSCOtherGroupItemLanguageRepository.GetPolicy24HSCOtherGroupItemLanguage(policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage.Policy24HSCOtherGroupItemLanguageId);

			//Check Exists
			if (policy24HSCOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<Policy24HSCOtherGroupItemLanguage>(policy24HSCOtherGroupItemLanguage, "Policy24HSCOtherGroupItemLanguage");
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
				policy24HSCOtherGroupItemLanguageRepository.Edit(policy24HSCOtherGroupItemLanguageVM);
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
					ViewData["ReturnURL"] = "/Policy24HSCOtherGroupItemLanguage.mvc/Edit/" + policy24HSCOtherGroupItemLanguage.Policy24HSCOtherGroupItemLanguageId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = policy24HSCOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policy24HSCOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			Policy24HSCOtherGroupItemLanguageVM policy24HSCOtherGroupItemLanguageVM = new Policy24HSCOtherGroupItemLanguageVM();

			//Check Item Exists
			Policy24HSCOtherGroupItemLanguage policy24HSCOtherGroupItemLanguage = new Policy24HSCOtherGroupItemLanguage();
			policy24HSCOtherGroupItemLanguage = policy24HSCOtherGroupItemLanguageRepository.GetPolicy24HSCOtherGroupItemLanguage(id);
			if (policy24HSCOtherGroupItemLanguage == null)
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
	
			policy24HSCOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage = policy24HSCOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policy24HSCOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			return View(policy24HSCOtherGroupItemLanguageVM);
		}

		// POST: /Policy24HSCOtherGroupItemLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(Policy24HSCOtherGroupItemLanguageVM policy24HSCOtherGroupItemLanguageVM)
		{
			//Check Valid Item passed in Form       
			if (policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			Policy24HSCOtherGroupItemLanguage policy24HSCOtherGroupItemLanguage = new Policy24HSCOtherGroupItemLanguage();
			policy24HSCOtherGroupItemLanguage = policy24HSCOtherGroupItemLanguageRepository.GetPolicy24HSCOtherGroupItemLanguage(policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage.Policy24HSCOtherGroupItemLanguageId);

			//Check Exists
			if (policy24HSCOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
		
			//Delete Form Item
			try
			{
				policy24HSCOtherGroupItemLanguageRepository.Delete(policy24HSCOtherGroupItemLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/Policy24HSCOtherGroupItemLanguage.mvc/Delete/" + policy24HSCOtherGroupItemLanguageVM.Policy24HSCOtherGroupItemLanguage.Policy24HSCOtherGroupItemLanguageId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policy24HSCOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policy24HSCOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}
	}
}
