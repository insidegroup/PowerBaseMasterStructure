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
	public class PolicyAllOtherGroupItemLanguageController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyAllOtherGroupItemLanguageRepository policyAllOtherGroupItemLanguageRepository = new PolicyAllOtherGroupItemLanguageRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /PolicyAllOtherGroupItemLanguage/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyAllOtherGroupItemLanguagesVM policyAllOtherGroupItemLanguagesVM = new PolicyAllOtherGroupItemLanguagesVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				return View("Error");
			}

			policyAllOtherGroupItemLanguagesVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				return View("Error");
			}

			policyAllOtherGroupItemLanguagesVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policyAllOtherGroupItemLanguagesVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policyAllOtherGroupItemLanguagesVM.HasWriteAccess = true;
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

			policyAllOtherGroupItemLanguagesVM.PolicyAllVendorGroupItemLanguages = policyAllOtherGroupItemLanguageRepository.GetPolicyAllOtherGroupItemLanguages(id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);

			return View(policyAllOtherGroupItemLanguagesVM);
		}

		// GET: /PolicyAllOtherGroupItemLanguage/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyAllOtherGroupItemLanguageVM policyAllOtherGroupItemLanguageVM = new PolicyAllOtherGroupItemLanguageVM();

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

			policyAllOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAllOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			PolicyAllOtherGroupItemLanguage policyAllOtherGroupItemLanguage = new PolicyAllOtherGroupItemLanguage();
			policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage = policyAllOtherGroupItemLanguage;

			//Languages
			SelectList languages = new SelectList(policyAllOtherGroupItemLanguageRepository.GetAvailableLanguages(id, policyGroupId).ToList(), "LanguageCode", "LanguageName");
			policyAllOtherGroupItemLanguageVM.Languages = languages;

			return View(policyAllOtherGroupItemLanguageVM);
		}

		// POST: /PolicyAllOtherGroupItemLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyAllOtherGroupItemLanguageVM policyAllOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyAllOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyAllOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			PolicyAllOtherGroupItemLanguage policyAllOtherGroupItemLanguage = new PolicyAllOtherGroupItemLanguage();
			policyAllOtherGroupItemLanguage = policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage;
			if (policyAllOtherGroupItemLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyAllOtherGroupItemLanguage>(policyAllOtherGroupItemLanguage, "PolicyAllOtherGroupItemLanguage");
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
				policyAllOtherGroupItemLanguageRepository.Add(policyAllOtherGroupItemLanguageVM);
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
			return RedirectToAction("List", new { id = policyAllOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAllOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyAllOtherGroupItemLanguage/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyAllOtherGroupItemLanguageVM policyAllOtherGroupItemLanguageVM = new PolicyAllOtherGroupItemLanguageVM();

			//Check Item Exists
			PolicyAllOtherGroupItemLanguage policyAllOtherGroupItemLanguage = new PolicyAllOtherGroupItemLanguage();
			policyAllOtherGroupItemLanguage = policyAllOtherGroupItemLanguageRepository.GetPolicyAllOtherGroupItemLanguage(id);
			if (policyAllOtherGroupItemLanguage == null)
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
	
			policyAllOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage = policyAllOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAllOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			//Languages
			List<Language> availableLanguages = policyAllOtherGroupItemLanguageRepository.GetAvailableLanguages(policyOtherGroupHeaderId, policyGroup.PolicyGroupId).ToList();
			Language selectedLanguage = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			selectedLanguage = languageRepository.GetLanguage(policyAllOtherGroupItemLanguage.LanguageCode);
			if (selectedLanguage != null)
			{
				availableLanguages.Add(selectedLanguage);
			}

			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageCode), "LanguageCode", "LanguageName", policyAllOtherGroupItemLanguage.LanguageCode);
			policyAllOtherGroupItemLanguageVM.Languages = languages;

			return View(policyAllOtherGroupItemLanguageVM);
		}

		// POST: /PolicyAllOtherGroupItemLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyAllOtherGroupItemLanguageVM policyAllOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Get Item
			PolicyAllOtherGroupItemLanguage policyAllOtherGroupItemLanguage = new PolicyAllOtherGroupItemLanguage();
			policyAllOtherGroupItemLanguage = policyAllOtherGroupItemLanguageRepository.GetPolicyAllOtherGroupItemLanguage(policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage.PolicyAllOtherGroupItemLanguageId);

			//Check Exists
			if (policyAllOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyAllOtherGroupItemLanguage>(policyAllOtherGroupItemLanguage, "PolicyAllOtherGroupItemLanguage");
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
				policyAllOtherGroupItemLanguageRepository.Edit(policyAllOtherGroupItemLanguageVM);
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
					ViewData["ReturnURL"] = "/PolicyAllOtherGroupItemLanguage.mvc/Edit/" + policyAllOtherGroupItemLanguage.PolicyAllOtherGroupItemLanguageId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = policyAllOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAllOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyAllOtherGroupItemLanguageVM policyAllOtherGroupItemLanguageVM = new PolicyAllOtherGroupItemLanguageVM();

			//Check Item Exists
			PolicyAllOtherGroupItemLanguage policyAllOtherGroupItemLanguage = new PolicyAllOtherGroupItemLanguage();
			policyAllOtherGroupItemLanguage = policyAllOtherGroupItemLanguageRepository.GetPolicyAllOtherGroupItemLanguage(id);
			if (policyAllOtherGroupItemLanguage == null)
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
	
			policyAllOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage = policyAllOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyAllOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			return View(policyAllOtherGroupItemLanguageVM);
		}

		// POST: /PolicyAllOtherGroupItemLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyAllOtherGroupItemLanguageVM policyAllOtherGroupItemLanguageVM)
		{
			//Check Valid Item passed in Form       
			if (policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyAllOtherGroupItemLanguage policyAllOtherGroupItemLanguage = new PolicyAllOtherGroupItemLanguage();
			policyAllOtherGroupItemLanguage = policyAllOtherGroupItemLanguageRepository.GetPolicyAllOtherGroupItemLanguage(policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage.PolicyAllOtherGroupItemLanguageId);

			//Check Exists
			if (policyAllOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
		
			//Delete Form Item
			try
			{
				policyAllOtherGroupItemLanguageRepository.Delete(policyAllOtherGroupItemLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyAllOtherGroupItemLanguage.mvc/Delete/" + policyAllOtherGroupItemLanguageVM.PolicyAllOtherGroupItemLanguage.PolicyAllOtherGroupItemLanguageId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyAllOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyAllOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}
	}
}
