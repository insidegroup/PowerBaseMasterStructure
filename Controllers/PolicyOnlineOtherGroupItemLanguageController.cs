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
	public class PolicyOnlineOtherGroupItemLanguageController : Controller
	{
		// Main repository
		PolicyOtherGroupHeaderRepository policyOtherGroupHeaderRepository = new PolicyOtherGroupHeaderRepository();
		PolicyOnlineOtherGroupItemLanguageRepository PolicyOnlineOtherGroupItemLanguageRepository = new PolicyOnlineOtherGroupItemLanguageRepository();

		PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		RolesRepository rolesRepository = new RolesRepository();

		// GET: /PolicyOnlineOtherGroupItemLanguage/List
		public ActionResult List(int id, int policyGroupId, string filter, int? page, string sortField, int? sortOrder)
		{
			PolicyOnlineOtherGroupItemLanguagesVM PolicyOnlineOtherGroupItemLanguagesVM = new PolicyOnlineOtherGroupItemLanguagesVM();

			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(policyGroupId);
			if (policyGroup == null)
			{
				return View("Error");
			}

			PolicyOnlineOtherGroupItemLanguagesVM.PolicyGroup = policyGroup;

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(id);
			if (policyOtherGroupHeader == null)
			{
				return View("Error");
			}

			PolicyOnlineOtherGroupItemLanguagesVM.PolicyOtherGroupHeader = policyOtherGroupHeader;

			//Set Access Rights
			PolicyOnlineOtherGroupItemLanguagesVM.HasWriteAccess = false;
			RolesRepository rolesRepository = new RolesRepository();
			if (rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId) && rolesRepository.HasWriteAccessToPolicyOnlineOtherGroupItemRepository())
			{
				PolicyOnlineOtherGroupItemLanguagesVM.HasWriteAccess = true;
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

			PolicyOnlineOtherGroupItemLanguagesVM.PolicyOnlineOtherGroupItemLanguages = PolicyOnlineOtherGroupItemLanguageRepository.GetPolicyOnlineOtherGroupItemLanguages(id, policyGroup.PolicyGroupId, filter ?? "", sortField, sortOrder ?? 0, page ?? 1);

			return View(PolicyOnlineOtherGroupItemLanguagesVM);
		}

		// GET: /PolicyOnlineOtherGroupItemLanguage/Create
		public ActionResult Create(int id, int policyGroupId)
		{
			PolicyOnlineOtherGroupItemLanguageVM PolicyOnlineOtherGroupItemLanguageVM = new PolicyOnlineOtherGroupItemLanguageVM();

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

            //Set Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId) || !rolesRepository.HasWriteAccessToPolicyOnlineOtherGroupItemRepository())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			PolicyOnlineOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			PolicyOnlineOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			PolicyOnlineOtherGroupItemLanguage PolicyOnlineOtherGroupItemLanguage = new PolicyOnlineOtherGroupItemLanguage();
			PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage = PolicyOnlineOtherGroupItemLanguage;

			//Languages
			SelectList languages = new SelectList(PolicyOnlineOtherGroupItemLanguageRepository.GetAvailableLanguages(id, policyGroupId).ToList(), "LanguageCode", "LanguageName");
			PolicyOnlineOtherGroupItemLanguageVM.Languages = languages;

			return View(PolicyOnlineOtherGroupItemLanguageVM);
		}

		// POST: /PolicyOnlineOtherGroupItemLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(PolicyOnlineOtherGroupItemLanguageVM PolicyOnlineOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Check Policy Exists
			PolicyGroup policyGroup = new PolicyGroup();
			policyGroup = policyGroupRepository.GetGroup(PolicyOnlineOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId);
			if (policyGroup == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//Check PolicyOtherGroupHeader Exists
			PolicyOtherGroupHeader policyOtherGroupHeader = new PolicyOtherGroupHeader();
			policyOtherGroupHeader = policyOtherGroupHeaderRepository.GetPolicyOtherGroupHeader(PolicyOnlineOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId);
			if (policyOtherGroupHeader == null)
			{
				ViewData["ActionMethod"] = "CreatePost";
				return View("RecordDoesNotExistError");
			}

			//We need to extract group from groupVM
			PolicyOnlineOtherGroupItemLanguage PolicyOnlineOtherGroupItemLanguage = new PolicyOnlineOtherGroupItemLanguage();
			PolicyOnlineOtherGroupItemLanguage = PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage;
			if (PolicyOnlineOtherGroupItemLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOnlineOtherGroupItemLanguage>(PolicyOnlineOtherGroupItemLanguage, "PolicyOnlineOtherGroupItemLanguage");
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
				PolicyOnlineOtherGroupItemLanguageRepository.Add(PolicyOnlineOtherGroupItemLanguageVM);
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
			return RedirectToAction("List", new { id = PolicyOnlineOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = PolicyOnlineOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /PolicyOnlineOtherGroupItemLanguage/Edit
		public ActionResult Edit(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyOnlineOtherGroupItemLanguageVM PolicyOnlineOtherGroupItemLanguageVM = new PolicyOnlineOtherGroupItemLanguageVM();

			//Check Item Exists
			PolicyOnlineOtherGroupItemLanguage PolicyOnlineOtherGroupItemLanguage = new PolicyOnlineOtherGroupItemLanguage();
			PolicyOnlineOtherGroupItemLanguage = PolicyOnlineOtherGroupItemLanguageRepository.GetPolicyOnlineOtherGroupItemLanguage(id);
			if (PolicyOnlineOtherGroupItemLanguage == null)
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
	
			//Set Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId) || !rolesRepository.HasWriteAccessToPolicyOnlineOtherGroupItemRepository())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

            PolicyOnlineOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage = PolicyOnlineOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			PolicyOnlineOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			//Languages
			List<Language> availableLanguages = PolicyOnlineOtherGroupItemLanguageRepository.GetAvailableLanguages(policyOtherGroupHeaderId, policyGroup.PolicyGroupId).ToList();
			Language selectedLanguage = new Language();
			LanguageRepository languageRepository = new LanguageRepository();
			selectedLanguage = languageRepository.GetLanguage(PolicyOnlineOtherGroupItemLanguage.LanguageCode);
			if (selectedLanguage != null)
			{
				availableLanguages.Add(selectedLanguage);
			}

			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageCode), "LanguageCode", "LanguageName", PolicyOnlineOtherGroupItemLanguage.LanguageCode);
			PolicyOnlineOtherGroupItemLanguageVM.Languages = languages;

			return View(PolicyOnlineOtherGroupItemLanguageVM);
		}

		// POST: /PolicyOnlineOtherGroupItemLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(PolicyOnlineOtherGroupItemLanguageVM PolicyOnlineOtherGroupItemLanguageVM, FormCollection formCollection)
		{
			//Get Item
			PolicyOnlineOtherGroupItemLanguage PolicyOnlineOtherGroupItemLanguage = new PolicyOnlineOtherGroupItemLanguage();
			PolicyOnlineOtherGroupItemLanguage = PolicyOnlineOtherGroupItemLanguageRepository.GetPolicyOnlineOtherGroupItemLanguage(PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage.PolicyOnlineOtherGroupItemLanguageId);

			//Check Exists
			if (PolicyOnlineOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<PolicyOnlineOtherGroupItemLanguage>(PolicyOnlineOtherGroupItemLanguage, "PolicyOnlineOtherGroupItemLanguage");
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
				PolicyOnlineOtherGroupItemLanguageRepository.Edit(PolicyOnlineOtherGroupItemLanguageVM);
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
					ViewData["ReturnURL"] = "/PolicyOnlineOtherGroupItemLanguage.mvc/Edit/" + PolicyOnlineOtherGroupItemLanguage.PolicyOnlineOtherGroupItemLanguageId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = PolicyOnlineOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = PolicyOnlineOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}

		// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, int policyGroupId, int policyOtherGroupHeaderId)
		{
			PolicyOnlineOtherGroupItemLanguageVM PolicyOnlineOtherGroupItemLanguageVM = new PolicyOnlineOtherGroupItemLanguageVM();

			//Check Item Exists
			PolicyOnlineOtherGroupItemLanguage policyOnlineOtherGroupItemLanguage = new PolicyOnlineOtherGroupItemLanguage();
			policyOnlineOtherGroupItemLanguage = PolicyOnlineOtherGroupItemLanguageRepository.GetPolicyOnlineOtherGroupItemLanguage(id);
			if (policyOnlineOtherGroupItemLanguage == null)
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
	
            //Set Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToPolicyGroup(policyGroupId) || !rolesRepository.HasWriteAccessToPolicyOnlineOtherGroupItemRepository())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

            //If user does not have OnlineUserFlag and the OnlineSensitiveDataFlag is set to true on the PolicyOtherGroupHeader, then mask the translation
            if (policyOtherGroupHeader.OnlineSensitiveDataFlag == true)
            {
                string adminUserGuid = System.Web.HttpContext.Current.User.Identity.Name.Split(new[] { '|' })[0];
                SystemUserRepository systemUserRepository = new SystemUserRepository();
                SystemUser systemUser = systemUserRepository.GetUserBySystemUserGuid(adminUserGuid);
                if (systemUser != null && systemUser.OnlineUserFlag != true)
                {
                    policyOnlineOtherGroupItemLanguage.Translation = new string('*', 20);
                }
            }

            PolicyOnlineOtherGroupItemLanguageVM.PolicyOtherGroupHeader = policyOtherGroupHeader;
			PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage = policyOnlineOtherGroupItemLanguage;
			policyGroupRepository.EditGroupForDisplay(policyGroup);
			PolicyOnlineOtherGroupItemLanguageVM.PolicyGroup = policyGroup;

			return View(PolicyOnlineOtherGroupItemLanguageVM);
		}

		// POST: /PolicyOnlineOtherGroupItemLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(PolicyOnlineOtherGroupItemLanguageVM PolicyOnlineOtherGroupItemLanguageVM)
		{
			//Check Valid Item passed in Form       
			if (PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Get Item From Database
			PolicyOnlineOtherGroupItemLanguage PolicyOnlineOtherGroupItemLanguage = new PolicyOnlineOtherGroupItemLanguage();
			PolicyOnlineOtherGroupItemLanguage = PolicyOnlineOtherGroupItemLanguageRepository.GetPolicyOnlineOtherGroupItemLanguage(PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage.PolicyOnlineOtherGroupItemLanguageId);

			//Check Exists
			if (PolicyOnlineOtherGroupItemLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			//Delete Form Item
			try
			{
				PolicyOnlineOtherGroupItemLanguageRepository.Delete(PolicyOnlineOtherGroupItemLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/PolicyOnlineOtherGroupItemLanguage.mvc/Delete/" + PolicyOnlineOtherGroupItemLanguageVM.PolicyOnlineOtherGroupItemLanguage.PolicyOnlineOtherGroupItemLanguageId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = PolicyOnlineOtherGroupItemLanguageVM.PolicyOtherGroupHeader.PolicyOtherGroupHeaderId, policyGroupId = PolicyOnlineOtherGroupItemLanguageVM.PolicyGroup.PolicyGroupId });
		}
	}
}
