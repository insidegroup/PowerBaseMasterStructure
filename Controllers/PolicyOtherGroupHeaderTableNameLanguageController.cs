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
	public class PolicyOtherGroupHeaderTableNameLanguageController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyOtherGroupHeaderTableNameRepository policyOtherGroupHeaderTableNameRepository = new PolicyOtherGroupHeaderTableNameRepository();
		PolicyOtherGroupHeaderTableNameLanguageRepository policyOtherGroupHeaderTableNameLanguageRepository = new PolicyOtherGroupHeaderTableNameLanguageRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		private string groupName = "Policy Group Other Headers Administrator";

		//
		// GET: /PolicyOtherGroupHeaderTableNameLanguage/List

		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
		{
			//SortField
			if (sortField == string.Empty)
			{
				sortField = "GDSResponseId";
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

			PolicyOtherGroupHeaderTableNameLanguagesVM policyOtherGroupHeaderTableNameLanguagesVM = new PolicyOtherGroupHeaderTableNameLanguagesVM();

			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);

			//Check Exists
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			policyOtherGroupHeaderTableNameLanguagesVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				policyOtherGroupHeaderTableNameLanguagesVM.HasWriteAccess = true;
			}

			//Get Items
			if (policyOtherGroupHeaderTableNameLanguageRepository != null)
			{
				var policyOtherGroupHeaderTableNameLanguages = policyOtherGroupHeaderTableNameLanguageRepository.PagePolicyOtherGroupHeaderTableNameLanguages(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

				if (policyOtherGroupHeaderTableNameLanguages != null)
				{
					policyOtherGroupHeaderTableNameLanguagesVM.PolicyOtherGroupHeaderTableNameLanguages = policyOtherGroupHeaderTableNameLanguages;
				}
			}

			//return items
			return View(policyOtherGroupHeaderTableNameLanguagesVM);
		}

		//
		// GET: /PolicyOtherGroupHeaderTableNameLanguage/Create
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

			PolicyOtherGroupHeaderTableNameLanguageVM policyOtherGroupHeaderTableNameLanguageVM = new PolicyOtherGroupHeaderTableNameLanguageVM();
			policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			PolicyOtherGroupHeaderTableNameLanguage policyOtherGroupHeaderTableNameLanguage = new PolicyOtherGroupHeaderTableNameLanguage();
			policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage = policyOtherGroupHeaderTableNameLanguage;

			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
			policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableName(
				policyOtherGroupHeader.PolicyOtherGroupHeaderId
			);

			if (policyOtherGroupHeaderTableName != null)
			{
				policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			}

			//Languages
			SelectList languages = new SelectList(policyOtherGroupHeaderTableNameLanguageRepository.GetAvailableLanguages(id).ToList(), "LanguageCode", "LanguageName");
			policyOtherGroupHeaderTableNameLanguageVM.Languages = languages;

			return View(policyOtherGroupHeaderTableNameLanguageVM);
		}

		// POST: /PolicyOtherGroupHeaderTableNameLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyOtherGroupHeaderTableNameLanguageVM policyOtherGroupHeaderTableNameLanguageVM, FormCollection formCollection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);

			//Check Exists
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			PolicyOtherGroupHeaderTableNameLanguage policyOtherGroupHeaderTableNameLanguage = new PolicyOtherGroupHeaderTableNameLanguage();
			policyOtherGroupHeaderTableNameLanguage = policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage;
			if (policyOtherGroupHeaderTableNameLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOtherGroupHeaderTableNameLanguage>(policyOtherGroupHeaderTableNameLanguage, "PolicyOtherGroupHeaderTableNameLanguage");
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
				policyOtherGroupHeaderTableNameLanguageRepository.Add(policyOtherGroupHeaderTableNameLanguageVM);
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
			return RedirectToAction("List", new { id = policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId } );
		}

		// GET: /PolicyOtherGroupHeaderTableNameLanguage/Edit
		public ActionResult Edit(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Get Item From Database
			PolicyOtherGroupHeaderTableNameLanguage policyOtherGroupHeaderTableNameLanguage = new PolicyOtherGroupHeaderTableNameLanguage();
			policyOtherGroupHeaderTableNameLanguage = policyOtherGroupHeaderTableNameLanguageRepository.GetPolicyOtherGroupHeaderTableNameLanguage(id);
			
			//Check Exists
			if (policyOtherGroupHeaderTableNameLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			PolicyOtherGroupHeaderTableNameLanguageVM policyOtherGroupHeaderTableNameLanguageVM = new PolicyOtherGroupHeaderTableNameLanguageVM();
			policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage = policyOtherGroupHeaderTableNameLanguage;

			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
			policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableNameByPolicyOtherGroupHeaderTableNameId(
				policyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameId
			);
			if (policyOtherGroupHeaderTableName != null)
			{
				policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			}

			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader != null)
			{
				policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			} 
			
			//Languages
			List<Language> availableLanguages = policyOtherGroupHeaderTableNameLanguageRepository.GetAvailableLanguages(policyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId).ToList();
			Language selectedLanguage = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			selectedLanguage = languageRepository.GetLanguage(policyOtherGroupHeaderTableNameLanguage.LanguageCode);
			if(selectedLanguage != null) {
				availableLanguages.Add(selectedLanguage);
			}

			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageCode), "LanguageCode", "LanguageName", policyOtherGroupHeaderTableNameLanguage.LanguageCode);
			policyOtherGroupHeaderTableNameLanguageVM.Languages = languages;

			return View(policyOtherGroupHeaderTableNameLanguageVM);
		}

		// POST: /PolicyOtherGroupHeaderTableNameLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyOtherGroupHeaderTableNameLanguageVM policyOtherGroupHeaderTableNameLanguageVM, FormCollection formCollection)
		{
			//Get Item
			PolicyOtherGroupHeaderTableNameLanguage policyOtherGroupHeaderTableNameLanguage = new PolicyOtherGroupHeaderTableNameLanguage();
			policyOtherGroupHeaderTableNameLanguage = policyOtherGroupHeaderTableNameLanguageRepository.GetPolicyOtherGroupHeaderTableNameLanguage(policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameLanguageId);

			//Check Exists
			if (policyOtherGroupHeaderTableNameLanguage == null)
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
				UpdateModel<PolicyOtherGroupHeaderTableNameLanguage>(policyOtherGroupHeaderTableNameLanguage, "PolicyOtherGroupHeaderTableNameLanguage");
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
				policyOtherGroupHeaderTableNameLanguageRepository.Edit(policyOtherGroupHeaderTableNameLanguage);
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
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeaderTableNameLanguage.mvc/Edit/" + policyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameLanguageId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Get Item From Database
			PolicyOtherGroupHeaderTableNameLanguage policyOtherGroupHeaderTableNameLanguage = new PolicyOtherGroupHeaderTableNameLanguage();
			policyOtherGroupHeaderTableNameLanguage = policyOtherGroupHeaderTableNameLanguageRepository.GetPolicyOtherGroupHeaderTableNameLanguage(id);

			//Check Exists
			if (policyOtherGroupHeaderTableNameLanguage == null)
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

			PolicyOtherGroupHeaderTableNameLanguageVM policyOtherGroupHeaderTableNameLanguageVM = new PolicyOtherGroupHeaderTableNameLanguageVM();
			policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage = policyOtherGroupHeaderTableNameLanguage;

			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
			policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableNameByPolicyOtherGroupHeaderTableNameId(
				policyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameId
			);
			if (policyOtherGroupHeaderTableName != null)
			{
				policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			}

			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader != null)
			{
				policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			} 

			return View(policyOtherGroupHeaderTableNameLanguageVM);
		}

		// POST: /PolicyOtherGroupHeaderTableNameLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyOtherGroupHeaderTableNameLanguageVM policyOtherGroupHeaderTableNameLanguageVM)
		{
			//Check Valid Item passed in Form       
			if (policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyOtherGroupHeaderTableNameLanguage policyOtherGroupHeaderTableNameLanguage = new PolicyOtherGroupHeaderTableNameLanguage();
			policyOtherGroupHeaderTableNameLanguage = policyOtherGroupHeaderTableNameLanguageRepository.GetPolicyOtherGroupHeaderTableNameLanguage(policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameLanguageId);

			//Check Exists
			if (policyOtherGroupHeaderTableNameLanguage == null)
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

			PolicyOtherGroupHeaderTableName policyOtherGroupHeaderTableName = new PolicyOtherGroupHeaderTableName();
			policyOtherGroupHeaderTableName = policyOtherGroupHeaderTableNameRepository.GetPolicyOtherGroupHeaderTableNameByPolicyOtherGroupHeaderTableNameId(
				policyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameId
			);
			if (policyOtherGroupHeaderTableName != null)
			{
				policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableName = policyOtherGroupHeaderTableName;
			}

			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(policyOtherGroupHeaderTableName.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader != null)
			{
				policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			} 

			//Delete Form Item
			try
			{
				policyOtherGroupHeaderTableNameLanguageRepository.Delete(policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOtherGroupHeaderTableNameLanguage.mvc/Delete/" + policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeaderTableNameLanguage.PolicyOtherGroupHeaderTableNameLanguageId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = policyOtherGroupHeaderTableNameLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId });
		}
	}
}
