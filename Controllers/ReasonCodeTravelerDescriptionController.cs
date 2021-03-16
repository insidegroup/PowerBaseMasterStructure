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
	public class ReasonCodeTravelerDescriptionController : Controller
    {
        //main repositories
        ReasonCodeTravelerDescriptionRepository reasonCodeTravelerDescriptionRepository = new ReasonCodeTravelerDescriptionRepository();
        ReasonCodeItemRepository reasonCodeItemRepository = new ReasonCodeItemRepository();
        ReasonCodeGroupRepository reasonCodeGroupRepository = new ReasonCodeGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get ReasonCodeItem
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);

            //Check Exists
            if (reasonCodeItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
            ViewData["ReasonCodeItem"] = reasonCodeItem.ReasonCode + "/" + reasonCodeItem.ReasonCodeTypeDescription + "/" + reasonCodeItem.ProductName;
            ViewData["ReasonCodeItemId"] = reasonCodeItem.ReasonCodeItemId;
            ViewData["ReasonCodeGroupId"] = reasonCodeItem.ReasonCodeGroupId;
            ViewData["ReasonCodeGroupName"] = reasonCodeGroupRepository.GetGroup(reasonCodeItem.ReasonCodeGroupId).ReasonCodeGroupName;
            ViewData["DisplayOrder"] = reasonCodeItem.DisplayOrder;

            //SortField+SortOrder settings
            if (sortField !="ReasonCodeTravelerDescription")
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

            //Get data
			var paginatedView = reasonCodeTravelerDescriptionRepository.GetTravelerDescriptions(id, page ?? 1, sortField, sortOrder ?? 0);

            //Return View
            return View(paginatedView);
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            //Get Item 
            ReasonCodeTravelerDescription reasonCodeTravelerDescription = new ReasonCodeTravelerDescription();
            reasonCodeTravelerDescription = reasonCodeTravelerDescriptionRepository.GetItem(id, languageCode);

            //Check Exists
            if (reasonCodeTravelerDescription == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Parent Information
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);
            reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
            ViewData["ReasonCodeItem"] = reasonCodeItem.ReasonCode + "/" + reasonCodeItem.ReasonCodeTypeDescription + "/" + reasonCodeItem.ProductName;
            ViewData["ReasonCodeItemId"] = reasonCodeItem.ReasonCodeItemId;
            ViewData["ReasonCodeGroupId"] = reasonCodeItem.ReasonCodeGroupId;
            ViewData["ReasonCodeGroupName"] = reasonCodeGroupRepository.GetGroup(reasonCodeItem.ReasonCodeGroupId).ReasonCodeGroupName;

            reasonCodeTravelerDescriptionRepository.EditItemForDisplay(reasonCodeTravelerDescription);
            return View(reasonCodeTravelerDescription);
        }


        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyAirVendorGroupItem
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);
            

            //Check Exists
            if (reasonCodeItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //New ReasonCodeTravelerDescription
            ReasonCodeTravelerDescription reasonCodeTravelerDescription = new ReasonCodeTravelerDescription();
            reasonCodeTravelerDescription.ReasonCodeItemId = id;
            
            //Language SelectList
            SelectList languageList = new SelectList(reasonCodeTravelerDescriptionRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Parent Information
            reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
            ViewData["ReasonCodeItem"] = reasonCodeItem.ReasonCode + "/" + reasonCodeItem.ReasonCodeTypeDescription + "/" + reasonCodeItem.ProductName;
            ViewData["ReasonCodeItemId"] = reasonCodeItem.ReasonCodeItemId;
            ViewData["ReasonCodeGroupId"] = reasonCodeItem.ReasonCodeGroupId;
			ViewData["ReasonCodeGroupName"] = reasonCodeGroupRepository.GetGroup(reasonCodeItem.ReasonCodeGroupId).ReasonCodeGroupName;
            
            //Show Create Form            
            reasonCodeTravelerDescriptionRepository.EditItemForDisplay(reasonCodeTravelerDescription);
            return View(reasonCodeTravelerDescription);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ReasonCodeTravelerDescription reasonCodeTravelerDescription)
        {
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(reasonCodeTravelerDescription.ReasonCodeItemId);

            //Check Exists
            if (reasonCodeItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(reasonCodeTravelerDescription);
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


            try
            {
                reasonCodeTravelerDescriptionRepository.Add(reasonCodeTravelerDescription);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            return RedirectToAction("List", new { id = reasonCodeItem.ReasonCodeItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            ReasonCodeTravelerDescription reasonCodeTravelerDescription = new ReasonCodeTravelerDescription();
            reasonCodeTravelerDescription = reasonCodeTravelerDescriptionRepository.GetItem(id, languageCode);

            //Check Exists
            if (reasonCodeTravelerDescription == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(reasonCodeTravelerDescriptionRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Parent Information
            reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
            ViewData["ReasonCodeItem"] = reasonCodeItem.ReasonCode + "/" + reasonCodeItem.ReasonCodeTypeDescription + "/" + reasonCodeItem.ProductName;
            ViewData["ReasonCodeItemId"] = reasonCodeItem.ReasonCodeItemId;
            ViewData["ReasonCodeGroupId"] = reasonCodeItem.ReasonCodeGroupId;
            ViewData["ReasonCodeGroupName"] = reasonCodeGroupRepository.GetGroup(reasonCodeItem.ReasonCodeGroupId).ReasonCodeGroupName;

            reasonCodeTravelerDescriptionRepository.EditItemForDisplay(reasonCodeTravelerDescription);
            return View(reasonCodeTravelerDescription);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, string reasonCodeTravelerDescription1)
        {
            //Get Item 
            ReasonCodeTravelerDescription reasonCodeTravelerDescription = new ReasonCodeTravelerDescription();
            reasonCodeTravelerDescription = reasonCodeTravelerDescriptionRepository.GetItem(id, languageCode);

            //Check Exists
            if (reasonCodeTravelerDescription == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(reasonCodeTravelerDescription);
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


            //Update AirlineAdvice
            try
            {
                reasonCodeTravelerDescriptionRepository.Update(reasonCodeTravelerDescription);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ReasonCodeTravelerDescription.mvc/Edit/" + reasonCodeTravelerDescription.ReasonCodeItemId.ToString() + "/" + reasonCodeTravelerDescription.LanguageCode;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = reasonCodeItem.ReasonCodeItemId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            ReasonCodeTravelerDescription reasonCodeTravelerDescription = new ReasonCodeTravelerDescription();
            reasonCodeTravelerDescription = reasonCodeTravelerDescriptionRepository.GetItem(id, languageCode);

            //Check Exists
            if (reasonCodeTravelerDescription == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //ParentInformation
            reasonCodeItemRepository.EditItemForDisplay(reasonCodeItem);
            ViewData["ReasonCodeItem"] = reasonCodeItem.ReasonCode + "/" + reasonCodeItem.ReasonCodeTypeDescription + "/" + reasonCodeItem.ProductName;
            ViewData["ReasonCodeItemId"] = reasonCodeItem.ReasonCodeItemId;
            ViewData["ReasonCodeGroupId"] = reasonCodeItem.ReasonCodeGroupId;
            ViewData["ReasonCodeGroupName"] = reasonCodeGroupRepository.GetGroup(reasonCodeItem.ReasonCodeGroupId).ReasonCodeGroupName;

            //Add Linked Information
            reasonCodeTravelerDescriptionRepository.EditItemForDisplay(reasonCodeTravelerDescription);

            //Return View
            return View(reasonCodeTravelerDescription);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            ReasonCodeTravelerDescription reasonCodeTravelerDescription = new ReasonCodeTravelerDescription();
            reasonCodeTravelerDescription = reasonCodeTravelerDescriptionRepository.GetItem(id, languageCode);

            //Check Exists
            if (reasonCodeTravelerDescription == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            ReasonCodeItem reasonCodeItem = new ReasonCodeItem();
            reasonCodeItem = reasonCodeItemRepository.GetItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToReasonCodeGroup(reasonCodeItem.ReasonCodeGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete
            try
            {
                reasonCodeTravelerDescription.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                reasonCodeTravelerDescriptionRepository.Delete(reasonCodeTravelerDescription);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/ReasonCodeTravelerDescription.mvc/Delete/" + reasonCodeTravelerDescription.ReasonCodeItemId.ToString() + "/" + reasonCodeTravelerDescription.LanguageCode;
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }
            //Return
            return RedirectToAction("List", new { id = reasonCodeTravelerDescription.ReasonCodeItemId });
        }
    }
}
