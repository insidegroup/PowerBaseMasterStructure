using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System;

namespace CWTDesktopDatabase.Controllers
{
    public class OptionalFieldLanguageController : Controller
    {
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
		OptionalFieldRepository optionalFieldRepository = new	OptionalFieldRepository();
        OptionalFieldLanguageRepository optionalFieldLanguageRepository = new OptionalFieldLanguageRepository();
        private string groupName = "Passive Segment Builder";

        //GET: A list of OptionalFieldLanguages
	    public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
	    {
		    //Check Access Rights to Domain
		    ViewData["Access"] = "";
		    if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
		    {
			    ViewData["Access"] = "WriteAccess";
		    }

		    //SortField + SortOrder settings
		    if (sortField != "OptionalFieldLabel")
		    {
				sortField = "LanguageName";
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

		    OptionalField optionalField = new OptionalField();
			OptionalFieldRepository optionalFieldRespository = new OptionalFieldRepository();
		    optionalField = optionalFieldRespository.GetItem(id);

		    if (optionalField != null)
		    {
				ViewData["OptionalFieldId"] = optionalField.OptionalFieldId;
				ViewData["OptionalFieldName"] = optionalField.OptionalFieldName;
		    }

			var items = optionalFieldLanguageRepository.PageOptionalFieldLanguages(id, filter ?? "", page ?? 1, sortField, sortOrder ?? 0);

		    return View(items);
	    }

		// GET: Create A Single OptionalFieldLanguage
		public ActionResult Create(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldLanguageVM optionalFieldLanguageVM = new OptionalFieldLanguageVM();

			//Need the name for breadcrumbs
			OptionalFieldRepository optionalFieldRespository = new OptionalFieldRepository();
			OptionalField optionalField = optionalFieldRespository.GetItem(id);
			if (optionalField != null)
			{
				ViewData["OptionalFieldName"] = optionalField.OptionalFieldName;
			}

			//Set Default Option Size
			OptionalFieldLanguage optionalFieldLanguage = new OptionalFieldLanguage();
			optionalFieldLanguage.OptionalFieldId = id;
			optionalFieldLanguage.OptionalFieldSize = 0;
			optionalFieldLanguageVM.OptionalFieldLanguage = optionalFieldLanguage;

			//Languages
			optionalFieldLanguageVM.OptionalFieldLanguages = new SelectList(optionalFieldLanguageRepository.GetAllAvailableLanguages(id), "LanguageCode", "LanguageName");

			return View(optionalFieldLanguageVM);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(OptionalFieldLanguageVM optionalFieldLanguageVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update Model from Form
			try
			{
				UpdateModel(optionalFieldLanguageVM);
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
				optionalFieldLanguageRepository.Add(optionalFieldLanguageVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldId });
		}

		// GET: View A Single OptionalFieldLanguage
		public ActionResult View(int id, string languageCode)
		{
			OptionalFieldLanguage optionalFieldLanguage = optionalFieldLanguageRepository.GetItem(id, languageCode);

			//Check Exists
			if (optionalFieldLanguage == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			OptionalFieldLanguageVM optionalFieldLanguageVM = new OptionalFieldLanguageVM();
			optionalFieldLanguageVM.OptionalFieldLanguage = optionalFieldLanguage;

			//Need the name for breadcrumbs
			OptionalFieldRepository optionalFieldRespository = new OptionalFieldRepository();
			OptionalField optionalField = optionalFieldRespository.GetItem(id);
			if (optionalField != null)
			{
				ViewData["OptionalFieldName"] = optionalField.OptionalFieldName;
			}

			//Languages
			optionalFieldLanguageVM.OptionalFieldLanguages = new SelectList(optionalFieldLanguageRepository.GetAllAvailableLanguages(id), "LanguageCode", "LanguageName");

			return View(optionalFieldLanguageVM);

		}

		// GET: Edit A Single OptionalFieldValue
		public ActionResult Edit(int id, string languageCode)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldLanguage optionalFieldLanguage = optionalFieldLanguageRepository.GetItem(id, languageCode);

			//Check Exists
			if (optionalFieldLanguage == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			OptionalFieldLanguageVM optionalFieldLanguageVM = new OptionalFieldLanguageVM();

			//Get the Languages
			LanguageRepository languageRepository = new LanguageRepository();
			optionalFieldLanguageVM.OptionalFieldLanguages = new SelectList(
																languageRepository.GetAllLanguages().ToList(),
																"LanguageCode",
																"LanguageName",
																optionalFieldLanguage.LanguageCode
																);
				
			//Need the name for breadcrumbs
			OptionalFieldRepository optionalFieldRespository = new OptionalFieldRepository();
			OptionalField optionalField = optionalFieldRespository.GetItem(id);
			if (optionalField != null)
			{
				ViewData["OptionalFieldName"] = optionalField.OptionalFieldName;
			}

			optionalFieldLanguageVM.OptionalFieldLanguage = optionalFieldLanguage;

			return View(optionalFieldLanguageVM);

		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(OptionalFieldLanguageVM optionalFieldLanguageVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldLanguage optionalFieldLanguage = new OptionalFieldLanguage();
			optionalFieldLanguage = optionalFieldLanguageRepository.GetItem(
					optionalFieldLanguageVM.OptionalFieldLanguage.OptionalFieldId, 
					optionalFieldLanguageVM.OptionalFieldLanguage.LanguageCode
			);

			//Check Exists
			if (optionalFieldLanguage == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//Update Model from Form
			try
			{
				UpdateModel(optionalFieldLanguageVM);
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
				optionalFieldLanguageRepository.Update(optionalFieldLanguageVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = optionalFieldLanguage.OptionalFieldId });
		}
		
		// GET: Delete A Single OptionalField
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldLanguage optionalFieldLanguage = new OptionalFieldLanguage();
			optionalFieldLanguage = optionalFieldLanguageRepository.GetItem(id, languageCode);

			//Check Exists
			if (optionalFieldLanguage == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			OptionalFieldLanguageVM optionalFieldLanguageVM = new OptionalFieldLanguageVM();
			optionalFieldLanguageVM.OptionalFieldLanguage = optionalFieldLanguage;

			// Need the name for breadcrumbs
			OptionalFieldRepository optionalFieldRespository = new OptionalFieldRepository();
			OptionalField optionalField = optionalFieldRespository.GetItem(id);
			ViewData["OptionalFieldName"] = optionalField.OptionalFieldName;

			return View(optionalFieldLanguageVM);
		}

		// POST: //Delete/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, FormCollection collection)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldLanguage optionalFieldLanguage = new OptionalFieldLanguage();
			optionalFieldLanguage = optionalFieldLanguageRepository.GetItem(id, collection["OptionalFieldLanguage.LanguageCode"]);

			//Check Exists
			if (optionalFieldLanguage == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				optionalFieldLanguage.VersionNumber = Int32.Parse(collection["OptionalFieldLanguage.VersionNumber"]);
				optionalFieldLanguageRepository.Delete(optionalFieldLanguage);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/OptionalFieldLanguage.mvc/Delete/" + optionalFieldLanguage.OptionalFieldId.ToString();
					return View("VersionError");
				}
				//Restraint Error - go to standard DeleteError page
				if (ex.Message == "SQLDeleteError")
				{
					ViewData["ReturnURL"] = "/OptionalFieldLanguage.mvc/Delete/" + optionalFieldLanguage.OptionalFieldId.ToString();
					return View("DeleteError");
				}
				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List", new { id = optionalFieldLanguage.OptionalFieldId });
		}
    }
}
