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
	public class PolicyOtherGroupHeaderColumnNameLanguageController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyOtherGroupHeaderColumnNameRepository policyOtherGroupHeaderColumnNameRepository = new PolicyOtherGroupHeaderColumnNameRepository();
		PolicyOtherGroupHeaderTableNameRepository policyOtherGroupHeaderTableNameRepository = new PolicyOtherGroupHeaderTableNameRepository();
		PolicyOtherGroupHeaderColumnNameLanguageRepository policyOtherGroupHeaderColumnNameLanguageRepository = new PolicyOtherGroupHeaderColumnNameLanguageRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		private string groupName = "Policy Group Other Headers Administrator";

		//
		// GET: /PolicyOtherGroupHeaderColumnNameLanguage/List

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

			PolicyOtherGroupHeaderColumnNameLanguagesVM policyOtherGroupHeaderColumnNameLanguagesVM = new PolicyOtherGroupHeaderColumnNameLanguagesVM();

			//Get Column Name
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnName(id);
			if (policyOtherGroupHeaderColumnName != null)
			{
				policyOtherGroupHeaderColumnNameLanguagesVM.PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;
			}

			//Check Exists
			if (policyOtherGroupHeaderColumnName == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Get Table Name
			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
			policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableNameByPolicyOtherGroupHeaderTableNameId(policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderTableNameId);
			if (policyOtherGroupHeaderTableName != null)
			{
				policyOtherGroupHeaderColumnNameLanguagesVM.PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			}

			//Check Exists
			if (policyOtherGroupHeaderTableName == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Get Policy Header
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader != null)
			{
				policyOtherGroupHeaderColumnNameLanguagesVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			}

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				policyOtherGroupHeaderColumnNameLanguagesVM.HasWriteAccess = true;
			}

			//Get Items
			if (policyOtherGroupHeaderColumnNameLanguageRepository != null)
			{
				var policyOtherGroupHeaderColumnNameLanguages = policyOtherGroupHeaderColumnNameLanguageRepository.PagePolicyOtherGroupHeaderColumnNameLanguages(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

				if (policyOtherGroupHeaderColumnNameLanguages != null)
				{
					policyOtherGroupHeaderColumnNameLanguagesVM.PolicyOtherGroupHeaderColumnNameLanguages = policyOtherGroupHeaderColumnNameLanguages;
				}
			}

			//return items
			return View(policyOtherGroupHeaderColumnNameLanguagesVM);
		}

		//
		// GET: /PolicyOtherGroupHeaderColumnNameLanguage/Create
		public ActionResult Create(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			PolicyOtherGroupHeaderColumnNameLanguageVM policyOtherGroupHeaderColumnNameLanguageVM = new PolicyOtherGroupHeaderColumnNameLanguageVM();

			//Get Column Name
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnName(id);

			//Check Exists
			if (policyOtherGroupHeaderColumnName == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			PolicyOtherGroupHeaderColumnNameLanguage policyOtherGroupHeaderColumnNameLanguage = new PolicyOtherGroupHeaderColumnNameLanguage();
			policyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameId = policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId;
			policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnNameLanguage = policyOtherGroupHeaderColumnNameLanguage;
			policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;

			//Get Table Name
			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
			policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableNameByPolicyOtherGroupHeaderTableNameId(policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderTableNameId);

			//Check Exists
			if (policyOtherGroupHeaderTableName == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Get Policy Header
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;			

			//Languages
			SelectList languages = new SelectList(policyOtherGroupHeaderColumnNameLanguageRepository.GetAvailableLanguages(policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId).ToList(), "LanguageCode", "LanguageName");
			policyOtherGroupHeaderColumnNameLanguageVM.Languages = languages;

			return View(policyOtherGroupHeaderColumnNameLanguageVM);
		}

		// POST: /PolicyOtherGroupHeaderColumnNameLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyOtherGroupHeaderColumnNameLanguageVM policyOtherGroupHeaderColumnNameLanguageVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnName(
				policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId
			);
			if (policyOtherGroupHeaderColumnName != null)
			{
				policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;
			}

			//Check Exists
			if (policyOtherGroupHeaderColumnName == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
			
			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);

			//Check Exists
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
			
			//We need to extract group from groupVM
			PolicyOtherGroupHeaderColumnNameLanguage policyOtherGroupHeaderColumnNameLanguage = new PolicyOtherGroupHeaderColumnNameLanguage();
			policyOtherGroupHeaderColumnNameLanguage = policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnNameLanguage;
			if (policyOtherGroupHeaderColumnNameLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupHeaderColumnNameLanguage>(policyOtherGroupHeaderColumnNameLanguage, "PolicyOtherGroupHeaderColumnNameLanguage");
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
				policyOtherGroupHeaderColumnNameLanguageRepository.Add(policyOtherGroupHeaderColumnNameLanguage);
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
			return RedirectToAction("List", new { id = policyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameId } );
		}

		// GET: /PolicyOtherGroupHeaderColumnNameLanguage/Edit
		public ActionResult Edit(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			PolicyOtherGroupHeaderColumnNameLanguage policyOtherGroupHeaderColumnNameLanguage = new PolicyOtherGroupHeaderColumnNameLanguage();
			policyOtherGroupHeaderColumnNameLanguage = policyOtherGroupHeaderColumnNameLanguageRepository.GetPolicyOtherGroupHeaderColumnNameLanguage(id); 
			
			//Check Exists
			if (policyOtherGroupHeaderColumnNameLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			PolicyOtherGroupHeaderColumnNameLanguageVM policyOtherGroupHeaderColumnNameLanguageVM = new PolicyOtherGroupHeaderColumnNameLanguageVM();
			policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnNameLanguage = policyOtherGroupHeaderColumnNameLanguage;

			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnName(policyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameId);
			if (policyOtherGroupHeaderColumnName != null)
			{
				policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;

				//Get Table Name
				PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
				policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableNameByPolicyOtherGroupHeaderTableNameId(policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderTableNameId);
				if (policyOtherGroupHeaderTableName != null)
				{
					policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
				}

				//Get Policy Other Group Header
				PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
				policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId);
				if (policyOtherGroupHeader != null)
				{
					policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
				}
			}

			//Languages
			List<Language> availableLanguages = policyOtherGroupHeaderColumnNameLanguageRepository.GetAvailableLanguages(policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId).ToList();
			Language selectedLanguage = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			selectedLanguage = languageRepository.GetLanguage(policyOtherGroupHeaderColumnNameLanguage.LanguageCode);
			if(selectedLanguage != null) {
				availableLanguages.Add(selectedLanguage);
			}

			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageCode), "LanguageCode", "LanguageName", policyOtherGroupHeaderColumnNameLanguage.LanguageCode);
			policyOtherGroupHeaderColumnNameLanguageVM.Languages = languages;

			return View(policyOtherGroupHeaderColumnNameLanguageVM);
		}

		// POST: /PolicyOtherGroupHeaderColumnNameLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyOtherGroupHeaderColumnNameLanguageVM policyOtherGroupHeaderColumnNameLanguageVM, FormCollection formCollection)
		{
			//Get Item
			PolicyOtherGroupHeaderColumnNameLanguage policyOtherGroupHeaderColumnNameLanguage = new PolicyOtherGroupHeaderColumnNameLanguage();
			policyOtherGroupHeaderColumnNameLanguage = policyOtherGroupHeaderColumnNameLanguageRepository.GetPolicyOtherGroupHeaderColumnNameLanguage(policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameLanguageId);

			//Check Exists
			if (policyOtherGroupHeaderColumnNameLanguage == null)
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
				UpdateModel<PolicyOtherGroupHeaderColumnNameLanguage>(policyOtherGroupHeaderColumnNameLanguage, "PolicyOtherGroupHeaderColumnNameLanguage");
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
				policyOtherGroupHeaderColumnNameLanguageRepository.Edit(policyOtherGroupHeaderColumnNameLanguage);
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
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeaderColumnNameLanguage.mvc/Edit/" + policyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameLanguageId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderColumnNameId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			PolicyOtherGroupHeaderColumnNameLanguage policyOtherGroupHeaderColumnNameLanguage = new PolicyOtherGroupHeaderColumnNameLanguage();
			policyOtherGroupHeaderColumnNameLanguage = policyOtherGroupHeaderColumnNameLanguageRepository.GetPolicyOtherGroupHeaderColumnNameLanguage(id);

			//Check Exists
			if (policyOtherGroupHeaderColumnNameLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			PolicyOtherGroupHeaderColumnNameLanguageVM policyOtherGroupHeaderColumnNameLanguageVM = new PolicyOtherGroupHeaderColumnNameLanguageVM();
			policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnNameLanguage = policyOtherGroupHeaderColumnNameLanguage;

			PolicyOtherGroupHeaderColumnName policyOtherGroupHeaderColumnName = new PolicyOtherGroupHeaderColumnName();
			policyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnNameRepository.GetPolicyOtherGroupHeaderColumnName(policyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameId);
			if (policyOtherGroupHeaderColumnName != null)
			{
				policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnName = policyOtherGroupHeaderColumnName;

				//Get Table Name
				PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
				policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableNameByPolicyOtherGroupHeaderTableNameId(policyOtherGroupHeaderColumnName.PolicyOtherGroupHeaderTableNameId);

				//Get Policy Other Group Header
				PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
				policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId);
				if (policyOtherGroupHeader != null)
				{
					policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
				}
			}

			return View(policyOtherGroupHeaderColumnNameLanguageVM);
		}

		// POST: /PolicyOtherGroupHeaderColumnNameLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyOtherGroupHeaderColumnNameLanguageVM policyOtherGroupHeaderColumnNameLanguageVM)
		{
			//Check Valid Item passed in Form       
			if (policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnNameLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyOtherGroupHeaderColumnNameLanguage policyOtherGroupHeaderColumnNameLanguage = new PolicyOtherGroupHeaderColumnNameLanguage();
			policyOtherGroupHeaderColumnNameLanguage = policyOtherGroupHeaderColumnNameLanguageRepository.GetPolicyOtherGroupHeaderColumnNameLanguage(policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameLanguageId);

			//Check Exists
			if (policyOtherGroupHeaderColumnNameLanguage == null)
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

			//Delete Form Item
			try
			{
				policyOtherGroupHeaderColumnNameLanguageRepository.Delete(policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnNameLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeaderColumnNameLanguage.mvc/Delete/" + policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameLanguageId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyOtherGroupHeaderColumnNameLanguageVM.PolicyOtherGroupHeaderColumnNameLanguage.PolicyOtherGroupHeaderColumnNameId });
		}
	}
}
