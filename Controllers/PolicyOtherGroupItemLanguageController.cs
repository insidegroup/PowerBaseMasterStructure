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
	public class PolicyOtherGroupItemLanguageController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyOtherGroupItemLanguageRepository policyOtherGroupItemLanguageRepository = new PolicyOtherGroupItemLanguageRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /PolicyOtherGroupItemLanguage/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyOtherGroupItemLanguagesVM policyOtherGroupItemLanguagesVM = new PolicyOtherGroupItemLanguagesVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				return View("Error");
			}

			policyOtherGroupItemLanguagesVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				return View("Error");
			}

			policyOtherGroupItemLanguagesVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policyOtherGroupItemLanguagesVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policyOtherGroupItemLanguagesVM.HasWriteAccess = true;
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

			policyOtherGroupItemLanguagesVM.PolicyVendorGroupItemLanguages = policyOtherGroupItemLanguageRepository.GetPolicyOtherGroupItemLanguages(id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);

			return View(policyOtherGroupItemLanguagesVM);
		}

		// GET: /PolicyOtherGroupItemLanguage/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyOtherGroupItemLanguageVM policyOtherGroupItemLanguageVM = new PolicyOtherGroupItemLanguageVM();

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

			policyOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			PolicyOtherGroupItemLanguage policyOtherGroupItemLanguage = new PolicyOtherGroupItemLanguage();
			policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage = policyOtherGroupItemLanguage;

			//Languages
			SelectList languages = new SelectList(policyOtherGroupItemLanguageRepository.GetAvailableLanguages(id, policyGroupId).ToList(), "LanguageCode", "LanguageName");
			policyOtherGroupItemLanguageVM.Languages = languages;

			return View(policyOtherGroupItemLanguageVM);
		}

		// POST: /PolicyOtherGroupItemLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyOtherGroupItemLanguageVM policyOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			PolicyOtherGroupItemLanguage policyOtherGroupItemLanguage = new PolicyOtherGroupItemLanguage();
			policyOtherGroupItemLanguage = policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage;
			if (policyOtherGroupItemLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupItemLanguage>(policyOtherGroupItemLanguage, "PolicyOtherGroupItemLanguage");
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
				policyOtherGroupItemLanguageRepository.Add(policyOtherGroupItemLanguageVM);
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
			return RedirectToAction("List", new { id = policyOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyOtherGroupItemLanguage/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyOtherGroupItemLanguageVM policyOtherGroupItemLanguageVM = new PolicyOtherGroupItemLanguageVM();

			//Check Item Exists
			PolicyOtherGroupItemLanguage policyOtherGroupItemLanguage = new PolicyOtherGroupItemLanguage();
			policyOtherGroupItemLanguage = policyOtherGroupItemLanguageRepository.GetPolicyOtherGroupItemLanguage(id);
			if (policyOtherGroupItemLanguage == null)
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
	
			policyOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage = policyOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			//Languages
			List<Language> availableLanguages = policyOtherGroupItemLanguageRepository.GetAvailableLanguages(policyOtherGroupHeaderId, policyGroup.PolicyGroupId).ToList();
			Language selectedLanguage = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			selectedLanguage = languageRepository.GetLanguage(policyOtherGroupItemLanguage.LanguageCode);
			if (selectedLanguage != null)
			{
				availableLanguages.Add(selectedLanguage);
			}

			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageCode), "LanguageCode", "LanguageName", policyOtherGroupItemLanguage.LanguageCode);
			policyOtherGroupItemLanguageVM.Languages = languages;

			return View(policyOtherGroupItemLanguageVM);
		}

		// POST: /PolicyOtherGroupItemLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyOtherGroupItemLanguageVM policyOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Get Item
			PolicyOtherGroupItemLanguage policyOtherGroupItemLanguage = new PolicyOtherGroupItemLanguage();
			policyOtherGroupItemLanguage = policyOtherGroupItemLanguageRepository.GetPolicyOtherGroupItemLanguage(policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage.PolicyOtherGroupItemLanguageId);

			//Check Exists
			if (policyOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupItemLanguage>(policyOtherGroupItemLanguage, "PolicyOtherGroupItemLanguage");
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
				policyOtherGroupItemLanguageRepository.Edit(policyOtherGroupItemLanguageVM);
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
					ViewData["ReturnURL"] = "/PolicyOtherGroupItemLanguage.mvc/Edit/" + policyOtherGroupItemLanguage.PolicyOtherGroupItemLanguageId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = policyOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyOtherGroupItemLanguageVM policyOtherGroupItemLanguageVM = new PolicyOtherGroupItemLanguageVM();

			//Check Item Exists
			PolicyOtherGroupItemLanguage policyOtherGroupItemLanguage = new PolicyOtherGroupItemLanguage();
			policyOtherGroupItemLanguage = policyOtherGroupItemLanguageRepository.GetPolicyOtherGroupItemLanguage(id);
			if (policyOtherGroupItemLanguage == null)
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
	
			policyOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage = policyOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			return View(policyOtherGroupItemLanguageVM);
		}

		// POST: /PolicyOtherGroupItemLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyOtherGroupItemLanguageVM policyOtherGroupItemLanguageVM)
		{
			//Check Valid Item passed in Form       
			if (policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyOtherGroupItemLanguage policyOtherGroupItemLanguage = new PolicyOtherGroupItemLanguage();
			policyOtherGroupItemLanguage = policyOtherGroupItemLanguageRepository.GetPolicyOtherGroupItemLanguage(policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage.PolicyOtherGroupItemLanguageId);

			//Check Exists
			if (policyOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
		
			//Delete Form Item
			try
			{
				policyOtherGroupItemLanguageRepository.Delete(policyOtherGroupItemLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupItemLanguage.mvc/Delete/" + policyOtherGroupItemLanguageVM.PolicyOtherGroupItemLanguage.PolicyOtherGroupItemLanguageId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}
	}
}
