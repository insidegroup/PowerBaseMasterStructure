using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.Repository;
using CWTDesktopDatabase.Helpers;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class ContactTypeController : Controller
    {
        ContactTypeRepository contactTypeRepository = new ContactTypeRepository();

        // GET: /List
         public ActionResult List(int? page)
         {
             //Set Access Rights
             ViewData["Access"] = "";
             RolesRepository rolesRepository = new RolesRepository();
             if (rolesRepository.HasWriteAccessToReferenceInfo())
             {
                 ViewData["Access"] = "WriteAccess";
             }
             ViewData["CurrentSortField"] = "Name";
             ViewData["CurrentSortOrder"] = 0;

             //return items
             var cwtPaginatedList = contactTypeRepository.PageContactTypes(page ?? 1);
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


             ContactType contactType = new ContactType();
             return View(contactType);
         }

         // POST: /Create
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Create(ContactType contactType)
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
                 UpdateModel(contactType);
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
                 contactTypeRepository.Add(contactType);
             }
             catch (SqlException ex)
             {
                 LogRepository logRepository = new LogRepository();
                 logRepository.LogError(ex.Message);

                 ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                 return View("Error");
             }

             ViewData["NewSortOrder"] = 0;
             return RedirectToAction("List");
         }

         // GET: /Edit
         public ActionResult Edit(int id)
         {
             ContactType contactType = new ContactType();
             contactType = contactTypeRepository.GetContactType(id);

             if (contactType == null)
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


             return View(contactType);
         }

         // POST: /Edit/
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Edit(int id, FormCollection collection)
         {

             //Get Item From Database
             ContactType contactType = new ContactType();
             contactType = contactTypeRepository.GetContactType(id);

             //Check Exists
             if (contactType == null)
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
                 UpdateModel(contactType);
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
                contactType.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                contactTypeRepository.Update(contactType);
            }
           catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ContactType.mvc/Edit/" + id.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

             //Success
             return RedirectToAction("List");

         }


         // GET: /Delete
		 [HttpGet]
		 public ActionResult Delete(int id)
         {
             //AccessRights
             RolesRepository rolesRepository = new RolesRepository();
             if (!rolesRepository.HasWriteAccessToReferenceInfo())
             {
                 ViewData["Message"] = "You do not have access to this item";
                 return View("Error");
             }


             ContactType contactType = new ContactType();
             contactType = contactTypeRepository.GetContactType(id);

             return View(contactType);
         }

         // POST: /Delete
         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Delete(int id, FormCollection collection)
         {
             //Get Item From Database
             ContactType contactType = new ContactType();
             contactType = contactTypeRepository.GetContactType(id);

             //Check Exists
             if (contactType == null)
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
                 contactType.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                 contactTypeRepository.Delete(contactType);
             }
             catch (SqlException ex)
             {
                 //Versioning Error - go to standard versionError page
                 if (ex.Message == "SQLVersioningError")
                 {
                     ViewData["ReturnURL"] = "/ContactType.mvc/Delete/" + id.ToString();
                     return View("VersionError");
                 }
                 if (ex.Message == "SQLDeleteErrorError")
                 {
                     ViewData["ReturnURL"] = "/ContactType.mvc/Delete/" + id.ToString();
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
