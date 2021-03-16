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
	public class GDSEndWarningConfigurationLanguageController : Controller
	{
		// Main repository
		GDSEndWarningConfigurationRepository gdsEndWarningConfigurationRepository = new GDSEndWarningConfigurationRepository();
		GDSEndWarningConfigurationLanguageRepository gdsEndWarningConfigurationLanguageRepository = new GDSEndWarningConfigurationLanguageRepository();
		HierarchyRepository hierarchyRepository = new HierarchyRepository();

		private string groupName = "GDS Response Message Administrator ";

		//
		// GET: /GDSEndWarningConfigurationLanguage/List

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

			GDSEndWarningConfigurationLanguagesVM GDSEndWarningConfigurationLanguagesVM = new GDSEndWarningConfigurationLanguagesVM();

			//Set Access Rights
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				GDSEndWarningConfigurationLanguagesVM.HasWriteAccess = true;
			}

			//Get GDSEndWarningConfigurationLanguages
			if (gdsEndWarningConfigurationLanguageRepository != null)
			{
				var GDSEndWarningConfigurationLanguages = gdsEndWarningConfigurationLanguageRepository.PageGDSEndWarningConfigurationLanguages(id, page ?? 1, filter ?? "", sortField, sortOrder ?? 0);

				if (GDSEndWarningConfigurationLanguages != null)
				{
					GDSEndWarningConfigurationLanguagesVM.GDSEndWarningConfigurationLanguages = GDSEndWarningConfigurationLanguages;
				}
			}

			//Get GDSEndWarningConfiguration From Database
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(id);

			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			GDSEndWarningConfigurationLanguagesVM.GDSEndWarningConfiguration = gdsEndWarningConfiguration;

			//return items
			return View(GDSEndWarningConfigurationLanguagesVM);
		}

		//
		// GET: /GDSEndWarningConfigurationLanguage/Create

		public ActionResult Create(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSEndWarningConfigurationLanguageVM gdsEndWarningConfigurationLanguageVM = new GDSEndWarningConfigurationLanguageVM();

			//Get GDSEndWarningConfiguration From Database
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(id);

			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfiguration = gdsEndWarningConfiguration;
			
			//Language List
			SelectList languages = new SelectList(gdsEndWarningConfigurationLanguageRepository.GetAllAvailableLanguages(id), "LanguageCode", "LanguageName");
			gdsEndWarningConfigurationLanguageVM.Languages = languages;

			GDSEndWarningConfigurationLanguage gdsEndWarningConfigurationLanguage = new GDSEndWarningConfigurationLanguage();
			gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId = id;
			gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguage;

			return View(gdsEndWarningConfigurationLanguageVM);
		}

		// POST: /GDSEndWarningConfigurationLanguage/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(GDSEndWarningConfigurationLanguageVM gdsEndWarningConfigurationLanguageVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//We need to extract group from groupVM
			GDSEndWarningConfigurationLanguage gdsEndWarningConfigurationLanguage = new GDSEndWarningConfigurationLanguage();
			gdsEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage;
			if (gdsEndWarningConfigurationLanguage == null)
			{
				ViewData["Message"] = "ValidationError : missing item"; ;
				return View("Error");
			}

			//Get GDSEndWarningConfiguration From Database
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId);

			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			gdsEndWarningConfigurationLanguage.GDSEndWarningConfiguration = gdsEndWarningConfiguration;
			gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage.GDSEndWarningConfigurationId = gdsEndWarningConfiguration.GDSEndWarningConfigurationId;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<GDSEndWarningConfigurationLanguage>(gdsEndWarningConfigurationLanguage, "GDSEndWarningConfigurationLanguage");
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
				gdsEndWarningConfigurationLanguageRepository.Add(gdsEndWarningConfigurationLanguage);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			ViewData["NewSortOrder"] = 0;

			return RedirectToAction("List", new { id = gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId });
		}

		// GET: /GDSEndWarningConfigurationLanguage/Edit
		public ActionResult Edit(int id, string languageCode)
		{
			//Get Item From Database
			GDSEndWarningConfigurationLanguage gdsEndWarningConfigurationLanguage = new GDSEndWarningConfigurationLanguage();
			gdsEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguageRepository.GetItem(id, languageCode);

			//Check Exists
			if (gdsEndWarningConfigurationLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}
			
			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToGDSEndWarningConfiguration())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			GDSEndWarningConfigurationLanguageVM gdsEndWarningConfigurationLanguageVM = new GDSEndWarningConfigurationLanguageVM();
			gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguage;

			//Get GDSEndWarningConfiguration From Database
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(id);

			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfiguration = gdsEndWarningConfiguration;

			//Language List - Get available plus current language
			List<Language> availableLanguages = gdsEndWarningConfigurationLanguageRepository.GetAllAvailableLanguages(id);
			
			LanguageRepository languageRepository = new LanguageRepository();
			Language selectedLanguage = languageRepository.GetLanguage(gdsEndWarningConfigurationLanguage.LanguageCode);
			if(selectedLanguage != null) {
				availableLanguages.Add(selectedLanguage);
			}
			
			SelectList languages = new SelectList(availableLanguages.OrderBy(x => x.LanguageName), "LanguageCode", "LanguageName", gdsEndWarningConfigurationLanguage.LanguageCode);
			
			gdsEndWarningConfigurationLanguageVM.Languages = languages;

			gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId = id;
			gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguage;

			return View(gdsEndWarningConfigurationLanguageVM);
		}

		// POST: /GDSEndWarningConfigurationLanguage/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(GDSEndWarningConfigurationLanguageVM gdsEndWarningConfigurationLanguageVM)
		{
			//Get Item
			GDSEndWarningConfigurationLanguage gdsEndWarningConfigurationLanguage = new GDSEndWarningConfigurationLanguage();
			gdsEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguageRepository.GetItem(
				gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage.GDSEndWarningConfigurationId,
				gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage.LanguageCode
			);

			//Check Exists
			if (gdsEndWarningConfigurationLanguage == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//Check Access Rights
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToGDSEndWarningConfiguration())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			string oldLanguageCode = gdsEndWarningConfigurationLanguage.LanguageCode;

			//Update Model From Form + Validate against DB
			try
			{
				UpdateModel<GDSEndWarningConfigurationLanguage>(gdsEndWarningConfigurationLanguage, "GDSEndWarningConfigurationLanguage");
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
				gdsEndWarningConfigurationLanguageRepository.Edit(gdsEndWarningConfigurationLanguage, oldLanguageCode);
			}
			catch (SqlException ex)
			{
				//Versioning Error
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GDSEndWarningConfigurationLanguage.mvc/Edit/" + gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId;
					return View("VersionError");
				}
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId });
		}

		// GET: /GDSEndWarningConfigurationLanguage/View
		public ActionResult View(int id, string languageCode)
		{
			GDSEndWarningConfigurationLanguage gdsEndWarningConfigurationLanguage = new GDSEndWarningConfigurationLanguage();
			gdsEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguageRepository.GetItem(id, languageCode);

			//Check Exists
			if (gdsEndWarningConfigurationLanguage == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			GDSEndWarningConfigurationLanguageVM gdsEndWarningConfigurationLanguageVM = new GDSEndWarningConfigurationLanguageVM();
			
			//Get GDSEndWarningConfiguration From Database
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId);

			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfiguration = gdsEndWarningConfiguration;
			gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguage;
			
			return View(gdsEndWarningConfigurationLanguageVM);
		}

		//// GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
		{
			GDSEndWarningConfigurationLanguage gdsEndWarningConfigurationLanguage = new GDSEndWarningConfigurationLanguage();
			gdsEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguageRepository.GetItem(id, languageCode);

			//Check Exists
			if (gdsEndWarningConfigurationLanguage == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			GDSEndWarningConfigurationLanguageVM gdsEndWarningConfigurationLanguageVM = new GDSEndWarningConfigurationLanguageVM();

			//Get GDSEndWarningConfiguration From Database
			GDSEndWarningConfiguration gdsEndWarningConfiguration = new GDSEndWarningConfiguration();
			gdsEndWarningConfiguration = gdsEndWarningConfigurationRepository.GetGroup(gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId);

			//Check Exists
			if (gdsEndWarningConfiguration == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfiguration = gdsEndWarningConfiguration;
			gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguage;

			return View(gdsEndWarningConfigurationLanguageVM);
		}

		// POST: /GDSEndWarningConfigurationLanguage/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(GDSEndWarningConfigurationLanguageVM gdsEndWarningConfigurationLanguageVM)
		{
			//Get Item
			GDSEndWarningConfigurationLanguage gdsEndWarningConfigurationLanguage = new GDSEndWarningConfigurationLanguage();
			gdsEndWarningConfigurationLanguage = gdsEndWarningConfigurationLanguageRepository.GetItem(
				gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage.GDSEndWarningConfigurationId,
				gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage.LanguageCode
			);
			//Check Exists
			if (gdsEndWarningConfigurationLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Check Access
			RolesRepository rolesRepository = new RolesRepository();
			if (!rolesRepository.HasWriteAccessToGDSEndWarningConfiguration())
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Delete Form Item
			try
			{
				gdsEndWarningConfigurationLanguageRepository.Delete(gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/GDSEndWarningConfigurationLanguage.mvc/Delete/" + gdsEndWarningConfigurationLanguageVM.GDSEndWarningConfigurationLanguage.GDSEndWarningConfigurationId;
					return View("VersionError");
				}

				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}
			return RedirectToAction("List", new { id = gdsEndWarningConfigurationLanguage.GDSEndWarningConfigurationId });
		}
	}
}
