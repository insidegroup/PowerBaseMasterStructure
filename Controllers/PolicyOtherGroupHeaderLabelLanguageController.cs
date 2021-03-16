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
	public class PolicyOtherGroupHeaderLabelLanguageController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyOtherGroupHeaderLabelRepository policyOtherGroupHeaderLabelRepository = new PolicyOtherGroupHeaderLabelRepository();
		PolicyOtherGroupHeaderLabelLanguageRepository policyOtherGroupHeaderLabelLanguageRepository = new PolicyOtherGroupHeaderLabelLanguageRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		private string groupName = "Policy Group Other Headers Administrator";

		//
		// GET: /PolicyOtherGroupHeaderLabelLanguage/List

		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (sortField == string.Empty)
			{
				sortField = "LanguageName";
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

			PolicyOtherGroupHeaderLabelLanguagesVM policyOtherGroupHeaderLabelLanguagesVM = new PolicyOtherGroupHeaderLabelLanguagesVM();

			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);

			//Check Exists
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			policyOtherGroupHeaderLabelLanguagesVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				policyOtherGroupHeaderLabelLanguagesVM.HasWriteAccess = true;
			}

			//Get Items
			if (policyOtherGroupHeaderLabelLanguageRepository != null)
			{
				var policyOtherGroupHeaderLabelLanguages = policyOtherGroupHeaderLabelLanguageRepository.PagePolicyOtherGroupHeaderLabelLanguages(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

				if (policyOtherGroupHeaderLabelLanguages != null)
				{
					policyOtherGroupHeaderLabelLanguagesVM.PolicyOtherGroupHeaderLabelLanguages = policyOtherGroupHeaderLabelLanguages;
				}
			}

			//return items
			return View(policyOtherGroupHeaderLabelLanguagesVM);
		}

		//
		// GET: /PolicyOtherGroupHeaderLabelLanguage/Create
		public ActionResult Create(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);

			//Check Exists
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			PolicyOtherGroupHeaderLabelLanguageVM policyOtherGroupHeaderLabelLanguageVM = new PolicyOtherGroupHeaderLabelLanguageVM();
			policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			PolicyOtherGroupHeaderLabelLanguage policyOtherGroupHeaderLabelLanguage = new PolicyOtherGroupHeaderLabelLanguage();
			policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage = policyOtherGroupHeaderLabelLanguage;

			PolicyOtherGroupHeaderLabel policyOtherGroupHeaderLabel = new PolicyOtherGroupHeaderLabel();
			policyOtherGroupHeaderLabel = policyOtherGroupHeaderLabelRepository.GetPolicyOtherGroupHeaderLabel(policyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeaderLabel != null)
			{
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabel = policyOtherGroupHeaderLabel;
			}

			//Languages
			SelectList languages = new SelectList(policyOtherGroupHeaderLabelLanguageRepository.GetAvailableLanguages(id).ToList(), "LanguageCode", "LanguageName");
			policyOtherGroupHeaderLabelLanguageVM.Languages = languages;

			return View(policyOtherGroupHeaderLabelLanguageVM);
		}

		// POST: /PolicyOtherGroupHeaderLabelLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyOtherGroupHeaderLabelLanguageVM policyOtherGroupHeaderLabelLanguageVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);

			//Check Exists
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			PolicyOtherGroupHeaderLabelLanguage policyOtherGroupHeaderLabelLanguage = new PolicyOtherGroupHeaderLabelLanguage();
			policyOtherGroupHeaderLabelLanguage = policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage;
			if (policyOtherGroupHeaderLabelLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupHeaderLabelLanguage>(policyOtherGroupHeaderLabelLanguage, "PolicyOtherGroupHeaderLabelLanguage");
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
				policyOtherGroupHeaderLabelLanguageRepository.Add(policyOtherGroupHeaderLabelLanguageVM);
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
			return RedirectToAction("List", new { id = policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId });
		}

		// GET: /PolicyOtherGroupHeaderLabelLanguage/Edit
		public ActionResult Edit(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			PolicyOtherGroupHeaderLabelLanguage policyOtherGroupHeaderLabelLanguage = new PolicyOtherGroupHeaderLabelLanguage();
			policyOtherGroupHeaderLabelLanguage = policyOtherGroupHeaderLabelLanguageRepository.GetPolicyOtherGroupHeaderLabelLanguage(id);
			
			//Check Exists
			if (policyOtherGroupHeaderLabelLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			PolicyOtherGroupHeaderLabelLanguageVM policyOtherGroupHeaderLabelLanguageVM = new PolicyOtherGroupHeaderLabelLanguageVM();
			policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage = policyOtherGroupHeaderLabelLanguage;

			PolicyOtherGroupHeaderLabel policyOtherGroupHeaderLabel = new PolicyOtherGroupHeaderLabel();
			policyOtherGroupHeaderLabel = policyOtherGroupHeaderLabelRepository.GetPolicyOtherGroupHeaderLabelByPolicyOtherGroupHeaderLabelId(
				policyOtherGroupHeaderLabelLanguage.PolicyOtherGroupHeaderLabelId
			);
			if (policyOtherGroupHeaderLabel != null)
			{
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabel = policyOtherGroupHeaderLabel;
			}

			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderLabel.PolicyOtherGroupHeaderId);
			if(policyOtherGroupHeader != null) {
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			}

			//Languages
			List<Language> availableLanguages = policyOtherGroupHeaderLabelLanguageRepository.GetAvailableLanguages(policyOtherGroupHeaderLabel.PolicyOtherGroupHeaderId).ToList();
			Language selectedLanguage = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			selectedLanguage = languageRepository.GetLanguage(policyOtherGroupHeaderLabelLanguage.LanguageCode);
			if(selectedLanguage != null) {
				availableLanguages.Add(selectedLanguage);
			}

			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageCode), "LanguageCode", "LanguageName", policyOtherGroupHeaderLabelLanguage.LanguageCode);
			policyOtherGroupHeaderLabelLanguageVM.Languages = languages;

			return View(policyOtherGroupHeaderLabelLanguageVM);
		}

		// POST: /PolicyOtherGroupHeaderLabelLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyOtherGroupHeaderLabelLanguageVM policyOtherGroupHeaderLabelLanguageVM, FormCollection formCollection)
		{
			//Get Item
			PolicyOtherGroupHeaderLabelLanguage policyOtherGroupHeaderLabelLanguage = new PolicyOtherGroupHeaderLabelLanguage();
			policyOtherGroupHeaderLabelLanguage = policyOtherGroupHeaderLabelLanguageRepository.GetPolicyOtherGroupHeaderLabelLanguage(policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage.PolicyOtherGroupHeaderLabelLanguageId);

			//Check Exists
			if (policyOtherGroupHeaderLabelLanguage == null)
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

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupHeaderLabelLanguage>(policyOtherGroupHeaderLabelLanguage, "PolicyOtherGroupHeaderLabelLanguage");
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
				policyOtherGroupHeaderLabelLanguageRepository.Edit(policyOtherGroupHeaderLabelLanguage);
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
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeaderLabelLanguage.mvc/Edit/" + policyOtherGroupHeaderLabelLanguage.PolicyOtherGroupHeaderLabelLanguageId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			PolicyOtherGroupHeaderLabelLanguage policyOtherGroupHeaderLabelLanguage = new PolicyOtherGroupHeaderLabelLanguage();
			policyOtherGroupHeaderLabelLanguage = policyOtherGroupHeaderLabelLanguageRepository.GetPolicyOtherGroupHeaderLabelLanguage(id);

			//Check Exists
			if (policyOtherGroupHeaderLabelLanguage == null)
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

			PolicyOtherGroupHeaderLabelLanguageVM policyOtherGroupHeaderLabelLanguageVM = new PolicyOtherGroupHeaderLabelLanguageVM();
			policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage = policyOtherGroupHeaderLabelLanguage;

			PolicyOtherGroupHeaderLabel policyOtherGroupHeaderLabel = new PolicyOtherGroupHeaderLabel();
			policyOtherGroupHeaderLabel = policyOtherGroupHeaderLabelRepository.GetPolicyOtherGroupHeaderLabelByPolicyOtherGroupHeaderLabelId(
				policyOtherGroupHeaderLabelLanguage.PolicyOtherGroupHeaderLabelId
			);
			if (policyOtherGroupHeaderLabel != null)
			{
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabel = policyOtherGroupHeaderLabel;
			}

			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderLabel.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader != null)
			{
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			}

			return View(policyOtherGroupHeaderLabelLanguageVM);
		}

		// POST: /PolicyOtherGroupHeaderLabelLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyOtherGroupHeaderLabelLanguageVM policyOtherGroupHeaderLabelLanguageVM)
		{
			//Check Valid Item passed in Form       
			if (policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyOtherGroupHeaderLabelLanguage policyOtherGroupHeaderLabelLanguage = new PolicyOtherGroupHeaderLabelLanguage();
			policyOtherGroupHeaderLabelLanguage = policyOtherGroupHeaderLabelLanguageRepository.GetPolicyOtherGroupHeaderLabelLanguage(
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage.PolicyOtherGroupHeaderLabelLanguageId
			);

			//Check Exists
			if (policyOtherGroupHeaderLabelLanguage == null)
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

			PolicyOtherGroupHeaderLabel policyOtherGroupHeaderLabel = new PolicyOtherGroupHeaderLabel();
			policyOtherGroupHeaderLabel = policyOtherGroupHeaderLabelRepository.GetPolicyOtherGroupHeaderLabelByPolicyOtherGroupHeaderLabelId(
				policyOtherGroupHeaderLabelLanguage.PolicyOtherGroupHeaderLabelId
			);
			if (policyOtherGroupHeaderLabel != null)
			{
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabel = policyOtherGroupHeaderLabel;
			}

			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderLabel.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader != null)
			{
				policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			}


			//Delete Form Item
			try
			{
				policyOtherGroupHeaderLabelLanguageRepository.Delete(policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeaderLabelLanguage.mvc/Delete/" + policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeaderLabelLanguage.PolicyOtherGroupHeaderLabelLanguageId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyOtherGroupHeaderLabelLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId });
		}
	}
}
