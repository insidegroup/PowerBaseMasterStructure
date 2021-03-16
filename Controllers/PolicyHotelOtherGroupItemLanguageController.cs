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
	public class PolicyHotelOtherGroupItemLanguageController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyHotelOtherGroupItemLanguageRepository policyHotelOtherGroupItemLanguageRepository = new PolicyHotelOtherGroupItemLanguageRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /PolicyHotelOtherGroupItemLanguage/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyHotelOtherGroupItemLanguagesVM policyHotelOtherGroupItemLanguagesVM = new PolicyHotelOtherGroupItemLanguagesVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				return View("Error");
			}

			policyHotelOtherGroupItemLanguagesVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				return View("Error");
			}

			policyHotelOtherGroupItemLanguagesVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			policyHotelOtherGroupItemLanguagesVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId))
			{
				policyHotelOtherGroupItemLanguagesVM.HasWriteAccess = true;
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

			policyHotelOtherGroupItemLanguagesVM.PolicyHotelVendorGroupItemLanguages = policyHotelOtherGroupItemLanguageRepository.GetPolicyHotelOtherGroupItemLanguages(id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);

			return View(policyHotelOtherGroupItemLanguagesVM);
		}

		// GET: /PolicyHotelOtherGroupItemLanguage/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyHotelOtherGroupItemLanguageVM policyHotelOtherGroupItemLanguageVM = new PolicyHotelOtherGroupItemLanguageVM();

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

			policyHotelOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyHotelOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			PolicyHotelOtherGroupItemLanguage policyHotelOtherGroupItemLanguage = new PolicyHotelOtherGroupItemLanguage();
			policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage = policyHotelOtherGroupItemLanguage;

			//Languages
			SelectList languages = new SelectList(policyHotelOtherGroupItemLanguageRepository.GetAvailableLanguages(id, policyGroupId).ToList(), "LanguageCode", "LanguageName");
			policyHotelOtherGroupItemLanguageVM.Languages = languages;

			return View(policyHotelOtherGroupItemLanguageVM);
		}

		// POST: /PolicyHotelOtherGroupItemLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyHotelOtherGroupItemLanguageVM policyHotelOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyHotelOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyHotelOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			PolicyHotelOtherGroupItemLanguage policyHotelOtherGroupItemLanguage = new PolicyHotelOtherGroupItemLanguage();
			policyHotelOtherGroupItemLanguage = policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage;
			if (policyHotelOtherGroupItemLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyHotelOtherGroupItemLanguage>(policyHotelOtherGroupItemLanguage, "PolicyHotelOtherGroupItemLanguage");
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
				policyHotelOtherGroupItemLanguageRepository.Add(policyHotelOtherGroupItemLanguageVM);
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
			return RedirectToAction("List", new { id = policyHotelOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyHotelOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyHotelOtherGroupItemLanguage/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyHotelOtherGroupItemLanguageVM policyHotelOtherGroupItemLanguageVM = new PolicyHotelOtherGroupItemLanguageVM();

			//Check Item Exists
			PolicyHotelOtherGroupItemLanguage policyHotelOtherGroupItemLanguage = new PolicyHotelOtherGroupItemLanguage();
			policyHotelOtherGroupItemLanguage = policyHotelOtherGroupItemLanguageRepository.GetPolicyHotelOtherGroupItemLanguage(id);
			if (policyHotelOtherGroupItemLanguage == null)
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
	
			policyHotelOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage = policyHotelOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyHotelOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			//Languages
			List<Language> availableLanguages = policyHotelOtherGroupItemLanguageRepository.GetAvailableLanguages(policyOtherGroupHeaderId, policyGroup.PolicyGroupId).ToList();
			Language selectedLanguage = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			selectedLanguage = languageRepository.GetLanguage(policyHotelOtherGroupItemLanguage.LanguageCode);
			if (selectedLanguage != null)
			{
				availableLanguages.Add(selectedLanguage);
			}

			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageCode), "LanguageCode", "LanguageName", policyHotelOtherGroupItemLanguage.LanguageCode);
			policyHotelOtherGroupItemLanguageVM.Languages = languages;

			return View(policyHotelOtherGroupItemLanguageVM);
		}

		// POST: /PolicyHotelOtherGroupItemLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyHotelOtherGroupItemLanguageVM policyHotelOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Get Item
			PolicyHotelOtherGroupItemLanguage policyHotelOtherGroupItemLanguage = new PolicyHotelOtherGroupItemLanguage();
			policyHotelOtherGroupItemLanguage = policyHotelOtherGroupItemLanguageRepository.GetPolicyHotelOtherGroupItemLanguage(policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage.PolicyHotelOtherGroupItemLanguageId);

			//Check Exists
			if (policyHotelOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyHotelOtherGroupItemLanguage>(policyHotelOtherGroupItemLanguage, "PolicyHotelOtherGroupItemLanguage");
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
				policyHotelOtherGroupItemLanguageRepository.Edit(policyHotelOtherGroupItemLanguageVM);
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
					ViewData["ReturnURL"] = "/PolicyHotelOtherGroupItemLanguage.mvc/Edit/" + policyHotelOtherGroupItemLanguage.PolicyHotelOtherGroupItemLanguageId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = policyHotelOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyHotelOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyHotelOtherGroupItemLanguageVM policyHotelOtherGroupItemLanguageVM = new PolicyHotelOtherGroupItemLanguageVM();

			//Check Item Exists
			PolicyHotelOtherGroupItemLanguage policyHotelOtherGroupItemLanguage = new PolicyHotelOtherGroupItemLanguage();
			policyHotelOtherGroupItemLanguage = policyHotelOtherGroupItemLanguageRepository.GetPolicyHotelOtherGroupItemLanguage(id);
			if (policyHotelOtherGroupItemLanguage == null)
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
	
			policyHotelOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage = policyHotelOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			policyHotelOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			return View(policyHotelOtherGroupItemLanguageVM);
		}

		// POST: /PolicyHotelOtherGroupItemLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyHotelOtherGroupItemLanguageVM policyHotelOtherGroupItemLanguageVM)
		{
			//Check Valid Item passed in Form       
			if (policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyHotelOtherGroupItemLanguage policyHotelOtherGroupItemLanguage = new PolicyHotelOtherGroupItemLanguage();
			policyHotelOtherGroupItemLanguage = policyHotelOtherGroupItemLanguageRepository.GetPolicyHotelOtherGroupItemLanguage(policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage.PolicyHotelOtherGroupItemLanguageId);

			//Check Exists
			if (policyHotelOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
		
			//Delete Form Item
			try
			{
				policyHotelOtherGroupItemLanguageRepository.Delete(policyHotelOtherGroupItemLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyHotelOtherGroupItemLanguage.mvc/Delete/" + policyHotelOtherGroupItemLanguageVM.PolicyHotelOtherGroupItemLanguage.PolicyHotelOtherGroupItemLanguageId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyHotelOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = policyHotelOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}
	}
}
