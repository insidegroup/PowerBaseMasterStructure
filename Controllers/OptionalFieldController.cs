using System;
using System.Linq;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class OptionalFieldController : Controller
    {
        HierarchyRepository hierarchyRepository = new HierarchyRepository();
        OptionalFieldRepository optionalFieldRepository = new OptionalFieldRepository();
        private string groupName = "Passive Segment Builder";

        //GET: A list of OptionalFields
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            //Check Access Rights to Domain
            ViewData["Access"] = "";
            if (hierarchyRepository.AdminHasDomainWriteAccess(groupName))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //SortField + SortOrder settings
            if (sortField != "'OptionalFieldStyleDescription")
            {
                sortField = "OptionalFieldName";
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

            var items = optionalFieldRepository.PageOptionalFields(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(items);
        }

        // GET: Create A Single OptionalField
	    public ActionResult Create()
	    {
		    //Check Access Rights to Domain
		    if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
		    {
			    ViewData["Message"] = "You do not have access to this item";
			    return View("Error");
		    }

		    OptionalFieldVM optionalFieldVM = new OptionalFieldVM();
			OptionalField optionalField = new OptionalField();

		    OptionalFieldStyle optionalFieldStyle = new OptionalFieldStyle();
			optionalField.OptionalFieldStyle = optionalFieldStyle;
			optionalFieldVM.OptionalField = optionalField;

		    OptionalFieldStyleRepository optionalFieldStyleRepository = new OptionalFieldStyleRepository();
			optionalFieldVM.OptionalFieldStyles = optionalFieldStyleRepository.GetAllOptionalFieldStyles().ToList();

			return View(optionalFieldVM);
	    }

		// POST: //Create/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(OptionalFieldVM optionalFieldVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			//Update  Model from Form
			try
			{
				UpdateModel(optionalFieldVM);
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
				optionalFieldRepository.Add(optionalFieldVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
		}

		// GET: View A Single OptionalField
		public ActionResult View(int id)
		{
			OptionalField optionalField = optionalFieldRepository.GetItem(id);

			//Check Exists
			if (optionalField == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			OptionalFieldVM optionalFieldVM = new OptionalFieldVM();

			//Get the Optional Field Style
			OptionalFieldStyleRepository optionalFieldStyleRepository = new OptionalFieldStyleRepository();
		
			optionalField.OptionalFieldStyle = optionalFieldStyleRepository.GetStyle(optionalField.OptionalFieldStyleId);

			optionalFieldVM.OptionalField = optionalField;

			return View(optionalFieldVM);
			
		}

		// GET: Edit A Single OptionalField
		public ActionResult Edit(int id)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalField optionalField = optionalFieldRepository.GetItem(id);

			//Check Exists
			if (optionalField == null)
			{
				ViewData["ActionMethod"] = "EditGet";
				return View("RecordDoesNotExistError");
			}

			OptionalFieldVM optionalFieldVM = new OptionalFieldVM();

			//Get the Optional Field Style
			OptionalFieldStyleRepository optionalFieldStyleRepository = new OptionalFieldStyleRepository();
			optionalFieldVM.OptionalFieldStyles = new SelectList(
																optionalFieldStyleRepository.GetAllOptionalFieldStylesQueryable().ToList(), 
																"OptionalFieldStyleId",
																"OptionalFieldStyleDescription", 
																optionalField.OptionalFieldStyleId
																);

			optionalFieldVM.OptionalField = optionalField;

			return View(optionalFieldVM);
			
		}

		// POST: //Edit/
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(OptionalFieldVM optionalFieldVM)
		{
			//Check Access Rights to Domain
			if (!hierarchyRepository.AdminHasDomainWriteAccess(groupName))
			{
				ViewData["Message"] = "You do not have access to this item";
				return View("Error");
			}

			OptionalField optionalField = new OptionalField();
			optionalField = optionalFieldRepository.GetItem(optionalFieldVM.OptionalField.OptionalFieldId);

			//Check Exists
			if (optionalField == null)
			{
				ViewData["ActionMethod"] = "EditPost";
				return View("RecordDoesNotExistError");
			}

			//Update Model from Form
			try
			{
				UpdateModel(optionalFieldVM);
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
				optionalFieldRepository.Update(optionalFieldVM);
			}
			catch (SqlException ex)
			{
				LogRepository logRepository = new LogRepository();
				logRepository.LogError(ex.Message);

				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			return RedirectToAction("List");
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

			OptionalField optionalField = new OptionalField();
			optionalField = optionalFieldRepository.GetItem(id);

			//Check Exists
			if (optionalField == null)
			{
				ViewData["ActionMethod"] = "ViewGet";
				return View("RecordDoesNotExistError");
			}

			OptionalFieldVM optionalFieldVM = new OptionalFieldVM();
			
			//Get the Optional Field Style
			OptionalFieldStyleRepository optionalFieldStyleRepository = new OptionalFieldStyleRepository();

			optionalField.OptionalFieldStyle = optionalFieldStyleRepository.GetStyle(optionalField.OptionalFieldStyleId);

			optionalFieldVM.OptionalField = optionalField;

			return View(optionalFieldVM);
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

			OptionalField optionalField = new OptionalField();
			optionalField = optionalFieldRepository.GetItem(id);

			//Check Exists
			if (optionalField == null)
			{
				ViewData["ActionMethod"] = "DeletePost";
				return View("RecordDoesNotExistError");
			}

			//Delete Item
			try
			{
				optionalField.VersionNumber = Int32.Parse(collection["OptionalField.VersionNumber"]);
				optionalFieldRepository.Delete(optionalField);
			}
			catch (SqlException ex)
			{
				//Versioning Error - go to standard versionError page
				if (ex.Message == "SQLVersioningError")
				{
					ViewData["ReturnURL"] = "/OptionalField.mvc/Delete/" + optionalField.OptionalFieldId.ToString();
					return View("VersionError");
				}
				//Restraint Error - go to standard DeleteError page
				if (ex.Message == "SQLDeleteError")
				{
					ViewData["ReturnURL"] = "/OptionalField.mvc/Delete/" + optionalField.OptionalFieldId.ToString();
					return View("DeleteError");
				}
				//Generic Error
				ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
				return View("Error");
			}

			//Return
			return RedirectToAction("List");
		}

    }
}