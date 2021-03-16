using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ControlValueController : Controller
    {
        //main repository
        ControlValueRepository controlValueRepository = new ControlValueRepository();

        // GET: /List/
        public ActionResult List(string filter, int? page, string sortField, int? sortOrder)
        {
            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Access"] = "WriteAccess";
            }

            ViewData["CurrentSortField"] = sortField;
            if (sortField != "ControlName" && sortField != "ControlValue")
            {
                sortField = "DisplayOrder";
                ViewData["CurrentSortField"] = "DisplayOrder";
            }

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

            //return items
            var cwtPaginatedList = controlValueRepository.PageControlValues(page ?? 1, filter ?? "", sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create()
        {
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ControlPropertyRepository controlPropertyRepository = new ControlPropertyRepository();
            SelectList controlPropertyList = new SelectList(controlPropertyRepository.GetAllControlProperties().ToList(), "ControlPropertyId", "ControlPropertyDescription");
            ViewData["ControlProperties"] = controlPropertyList;

            ControlNameRepository controlNameRepository = new ControlNameRepository();
            SelectList controlNameList = new SelectList(controlNameRepository.GetAllControlNames().ToList(), "ControlNameId", "ControlName1");
            ViewData["ControlNames"] = controlNameList;

            ControlValue controlValue = new ControlValue();
            return View(controlValue);
        }

        // POST: //Create/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ControlValue controlValue)
        {
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(controlValue);
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
                controlValueRepository.Add(controlValue);
            }
            catch
            {
                //Could not insert to database
                return View("Error");
            }

            ViewData["NewSortOrder"] = 0;
            return RedirectToAction("List");
        }

        // GET: /Edit
        public ActionResult Edit(int id)
        {
            //Check Exists
            ControlValue controlValue = new ControlValue();
            controlValue = controlValueRepository.GetControlValue(id);
            if (controlValue == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            ControlPropertyRepository controlPropertyRepository = new ControlPropertyRepository();
            SelectList controlPropertyList = new SelectList(controlPropertyRepository.GetAllControlProperties().ToList(), "ControlPropertyId", "ControlPropertyDescription");
            ViewData["ControlProperties"] = controlPropertyList;

            ControlNameRepository controlNameRepository = new ControlNameRepository();
            SelectList controlNameList = new SelectList(controlNameRepository.GetAllControlNames().ToList(), "ControlNameId", "ControlName1");
            ViewData["ControlNames"] = controlNameList;

            return View(controlValue);
        }

        // POST: /Edit/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection)
        {

            //Get Item From Database
            ControlValue controlValue = new ControlValue();
            controlValue = controlValueRepository.GetControlValue(id);

            //Check Exists
            if (controlValue == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }
            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Update Item from Form
            try
            {
                UpdateModel(controlValue);
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
                controlValueRepository.Update(controlValue);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ControlValue.mvc/Edit/" + controlValue.ControlValueId;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            //Success
            return RedirectToAction("List");

        }

        // GET: /View
        public ActionResult View(int id)
        {
            //Check Exists
            ControlValue controlValue = new ControlValue();
            controlValue = controlValueRepository.GetControlValue(id);
            if (controlValue == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            controlValueRepository.EditForDisplay(controlValue);
            return View(controlValue);
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id)
        {
            //Check Exists
            ControlValue controlValue = new ControlValue();
            controlValue = controlValueRepository.GetControlValue(id);
            if (controlValue == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            controlValueRepository.EditForDisplay(controlValue);
            return View(controlValue);
        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, FormCollection collection)
        {
            //Get Item From Database
           ControlValue controlValue = new ControlValue();
           controlValue = controlValueRepository.GetControlValue(id);

            //Check Exists
            if (controlValue == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReferenceInfo())
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }
            //Delete Item
            try
            {
                controlValue.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                controlValueRepository.Delete(controlValue);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ControlValue.mvc/Delete/" + controlValue.ControlValueId;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List");
        }

    }
}
