using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CWTDesktopDatabase.Controllers
{
	public class OptionalFieldLookupValueController : Controller
	{
		HierarchyRepository hierarchyRepository = new HierarchyRepository();
		OptionalFieldRepository optionalFieldRepository = new OptionalFieldRepository();
		OptionalFieldLookupValueRepository optionalFieldLookupValueRepository = new OptionalFieldLookupValueRepository();
		OptionalFieldLookupValueLanguageRepository optionalFieldLookupValueLanguageRepository = new OptionalFieldLookupValueLanguageRepository();
		private string groupName = "Passive Segment Builder";

		//GET: A list of OptionalFieldLookupValues
		public ActionResult List(int id, string filter, int? page, string sortField, int? sortOrder)
		{
			//Check Access Rights to Domain
			ViewData["Access"] = "";
			if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Access"] = "WriteAccess";
			}

			//SortField + SortOrder settings
			if (sortField != "LanguageName")
			{
				sortField = "OptionalFieldLookupValueLabel";
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

			var items = optionalFieldLookupValueRepository.PageOptionalFieldLookupValues(id, filter ?? "", page ?? 1, sortField, sortOrder ?? 0);

			return View(items);
		}

		// GET: Create A Single OptionalFieldLookupValue
		public ActionResult Create(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldLookupValueVM optionalFieldLookupValueVM = new OptionalFieldLookupValueVM();

			//Need the name for breadcrumbs
			OptionalFieldRepository optionalFieldRespository = new OptionalFieldRepository();
			OptionalField optionalField = optionalFieldRespository.GetItem(id);
			if (optionalField != null)
			{
				ViewData["OptionalFieldName"] = optionalField.OptionalFieldName;
			}

			OptionalFieldLookupValue optionalFieldLookupValue = new OptionalFieldLookupValue();
			optionalFieldLookupValue.OptionalFieldId = id;
			
			optionalFieldLookupValueVM.OptionalFieldLookupValue = optionalFieldLookupValue;

			//Languages
			LanguageRepository languageRepository = new LanguageRepository();
			optionalFieldLookupValueVM.OptionalFieldLookupValueLanguages = new SelectList(
																					languageRepository.GetAllLanguages().ToList(), 
																					"LanguageCode", 
																					"LanguageName"
																				);

			return View(optionalFieldLookupValueVM);
		}

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(OptionalFieldLookupValueVM optionalFieldLookupValueVM)
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
				UpdateModel(optionalFieldLookupValueVM);
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
				optionalFieldLookupValueRepository.Add(optionalFieldLookupValueVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = optionalFieldLookupValueVM.OptionalFieldLookupValue.OptionalFieldId });
		}

		// GET: View A Single OptionalFieldLookupValue
		public ActionResult View(int id)
		{
			OptionalFieldLookupValue optionalFieldLookupValue = optionalFieldLookupValueRepository.GetItem(id);

			//Check Exists
			if (optionalFieldLookupValue == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			OptionalFieldLookupValueVM optionalFieldLookupValueVM = new OptionalFieldLookupValueVM();
			optionalFieldLookupValueVM.OptionalFieldLookupValue = optionalFieldLookupValue;

			//Language
			OptionalFieldLookupValueLanguage optionalFieldLookupValueLanguage = new OptionalFieldLookupValueLanguage();
			optionalFieldLookupValueLanguage = optionalFieldLookupValueLanguageRepository.GetItem(optionalFieldLookupValue.OptionalFieldLookupValueId);
			if (optionalFieldLookupValueLanguage != null)
			{
				optionalFieldLookupValueVM.OptionalFieldLookupValueLanguage = optionalFieldLookupValueLanguage;
			}
	
			//Need the name for breadcrumbs
			OptionalFieldRepository optionalFieldRespository = new OptionalFieldRepository();
			OptionalField optionalField = optionalFieldRespository.GetItem(optionalFieldLookupValue.OptionalFieldId);
			if (optionalField != null)
			{
				ViewData["OptionalFieldName"] = optionalField.OptionalFieldName;
			}

			return View(optionalFieldLookupValueVM);

		}

		// GET: Edit A Single OptionalFieldLookupValue
		public ActionResult Edit(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldLookupValue optionalFieldLookupValue = optionalFieldLookupValueRepository.GetItem(id);

			//Check Exists
			if (optionalFieldLookupValue == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			OptionalFieldLookupValueVM optionalFieldLookupValueVM = new OptionalFieldLookupValueVM();

			//Language
			OptionalFieldLookupValueLanguage optionalFieldLookupValueLanguage = new OptionalFieldLookupValueLanguage();
			optionalFieldLookupValueLanguage = optionalFieldLookupValueLanguageRepository.GetItem(optionalFieldLookupValue.OptionalFieldLookupValueId);
			if (optionalFieldLookupValueLanguage != null)
			{
				optionalFieldLookupValueVM.OptionalFieldLookupValueLanguage = optionalFieldLookupValueLanguage;
			}
			
			//Get the Languages
			LanguageRepository languageRepository = new LanguageRepository();
			optionalFieldLookupValueVM.OptionalFieldLookupValueLanguages = new SelectList(
																					languageRepository.GetAllLanguages().ToList(),
																					"LanguageCode",
																					"LanguageName",
																					optionalFieldLookupValueLanguage.LanguageCode
																				);
		
			//Need the name for breadcrumbs
			OptionalFieldRepository optionalFieldRespository = new OptionalFieldRepository();
			OptionalField optionalField = optionalFieldRespository.GetItem(optionalFieldLookupValue.OptionalFieldId);
			if (optionalField != null)
			{
				ViewData["OptionalFieldName"] = optionalField.OptionalFieldName;
			}

			optionalFieldLookupValueVM.OptionalFieldLookupValue = optionalFieldLookupValue;

			return View(optionalFieldLookupValueVM);

		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(OptionalFieldLookupValueVM optionalFieldLookupValueVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldLookupValue optionalFieldLookupValue = new OptionalFieldLookupValue();
			optionalFieldLookupValue = optionalFieldLookupValueRepository.GetItem(
					optionalFieldLookupValueVM.OptionalFieldLookupValueLanguage.OptionalFieldLookupValueId
			);

			//Check Exists
			if (optionalFieldLookupValue == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//Update Model from Form
			try
			{
				UpdateModel(optionalFieldLookupValueVM);
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
				optionalFieldLookupValueRepository.Update(optionalFieldLookupValueVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List", new { id = optionalFieldLookupValue.OptionalFieldId });
		}

		// GET: Delete A Single OptionalField
		[HttpGet]
		public ActionResult Delete(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalFieldLookupValue optionalFieldLookupValue = new OptionalFieldLookupValue();
			optionalFieldLookupValue = optionalFieldLookupValueRepository.GetItem(id);

			//Check Exists
			if (optionalFieldLookupValue == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			OptionalFieldLookupValueVM optionalFieldLookupValueVM = new OptionalFieldLookupValueVM();
			optionalFieldLookupValueVM.OptionalFieldLookupValue = optionalFieldLookupValue;

			//Language
			OptionalFieldLookupValueLanguage optionalFieldLookupValueLanguage = new OptionalFieldLookupValueLanguage();
			optionalFieldLookupValueLanguage = optionalFieldLookupValueLanguageRepository.GetItem(optionalFieldLookupValue.OptionalFieldLookupValueId);
			if (optionalFieldLookupValueLanguage != null)
			{
				optionalFieldLookupValueVM.OptionalFieldLookupValueLanguage = optionalFieldLookupValueLanguage;
			}
			
			// Need the name for breadcrumbs
			OptionalFieldRepository optionalFieldRespository = new OptionalFieldRepository();
            OptionalField optionalField = optionalFieldRespository.GetItem(optionalFieldLookupValue.OptionalFieldId);
			ViewData["OptionalFieldName"] = optionalField.OptionalFieldName;

			return View(optionalFieldLookupValueVM);
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

			OptionalFieldLookupValue optionalFieldLookupValue = new OptionalFieldLookupValue();
			optionalFieldLookupValue = optionalFieldLookupValueRepository.GetItem(id);

			//Check Exists
			if (optionalFieldLookupValue == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				optionalFieldLookupValue.VersionNumber = Int32.Parse(collection["OptionalFieldLookupValueLanguage.VersionNumber"]);
				optionalFieldLookupValueRepository.Delete(optionalFieldLookupValue);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/OptionalFieldLookupValue.mvc/Delete/" + optionalFieldLookupValue.OptionalFieldId.ToString();
					return View("VersionError");
				}
				//Restraint Error - go to standard DeleteError page
				if (ex.Message == "SQLDeleteError")
				{
					ViewData["ReturnURL"] = "/OptionalFieldLookupValue.mvc/Delete/" + optionalFieldLookupValue.OptionalFieldId.ToString();
					return View("DeleteError");
				}
				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List", new { id = optionalFieldLookupValue.OptionalFieldId });
		}
	}
}