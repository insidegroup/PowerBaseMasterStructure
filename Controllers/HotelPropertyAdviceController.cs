using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CWTDesktopDatabase.Models;
using CWTDesktopDatabase.ViewModels;
using CWTDesktopDatabase.Helpers;
using CWTDesktopDatabase.Repository;
using System.Data.SqlClient;

namespace CWTDesktopDatabase.Controllers
{
    public class HotelPropertyAdviceController : Controller
    {
        //main repositories
        PolicyHotelPropertyGroupItemLanguageRepository policyHotelPropertyGroupItemLanguageRepository = new PolicyHotelPropertyGroupItemLanguageRepository();
        PolicyHotelPropertyGroupItemRepository policyHotelPropertyGroupItemRepository = new PolicyHotelPropertyGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyHotelPropertyGroupItem
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(id);

            //Check Exists
            if (policyHotelPropertyGroupItem == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyHotelPropertyGroupItemID"] = policyHotelPropertyGroupItem.PolicyHotelPropertyGroupItemId;
            ViewData["PolicyGroupID"] = policyHotelPropertyGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyHotelPropertyGroupItem.PolicyGroupId).PolicyGroupName;


            //SortField+SortOrder settings
            if (sortField != "HotelAdvice")
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
                sortOrder = 0;
            }

            //Get data
            var cwtPaginatedList = policyHotelPropertyGroupItemLanguageRepository.PagePolicyHotelPropertyGroupItemHotelAdvice(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage = new PolicyHotelPropertyGroupItemLanguage();
            policyHotelPropertyGroupItemLanguage = policyHotelPropertyGroupItemLanguageRepository.GetItem(id, languageCode);
            if (policyHotelPropertyGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            policyHotelPropertyGroupItemLanguageRepository.EditItemForDisplay(policyHotelPropertyGroupItemLanguage);
            return View(policyHotelPropertyGroupItemLanguage);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyHotelPropertyGroupItem
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(id);

            //Check Exists
            if (policyHotelPropertyGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //New PolicyHotelPropertyGroupItemLanguage
            PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage = new PolicyHotelPropertyGroupItemLanguage();
            policyHotelPropertyGroupItemLanguage.PolicyHotelPropertyGroupItemId = id;
            policyHotelPropertyGroupItemLanguageRepository.EditItemForDisplay(policyHotelPropertyGroupItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(policyHotelPropertyGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(policyHotelPropertyGroupItemLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage)
        {
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(policyHotelPropertyGroupItemLanguage.PolicyHotelPropertyGroupItemId);

            //Check Exists
            if (policyHotelPropertyGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(policyHotelPropertyGroupItemLanguage);
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
                policyHotelPropertyGroupItemLanguageRepository.Add(policyHotelPropertyGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyHotelPropertyGroupItem.PolicyHotelPropertyGroupItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage = new PolicyHotelPropertyGroupItemLanguage();
            policyHotelPropertyGroupItemLanguage = policyHotelPropertyGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyHotelPropertyGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(policyHotelPropertyGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            policyHotelPropertyGroupItemLanguageRepository.EditItemForDisplay(policyHotelPropertyGroupItemLanguage);
            return View(policyHotelPropertyGroupItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int policyHotelPropertyGroupItemId, string languageCode, string hotelAdvice)
        {
            //Get Item 
            PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage = new PolicyHotelPropertyGroupItemLanguage();
            policyHotelPropertyGroupItemLanguage = policyHotelPropertyGroupItemLanguageRepository.GetItem(policyHotelPropertyGroupItemId, languageCode);

            //Check Exists
            if (policyHotelPropertyGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(policyHotelPropertyGroupItemId);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyHotelPropertyGroupItemLanguage);
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

            //Update CountryAdvice
            try
            {
                policyHotelPropertyGroupItemLanguageRepository.Update(policyHotelPropertyGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/HotelPropertyAdvice.mvc/Edit/" + policyHotelPropertyGroupItemLanguage.PolicyHotelPropertyGroupItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            return RedirectToAction("List", new { id = policyHotelPropertyGroupItemLanguage.PolicyHotelPropertyGroupItemId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage = new PolicyHotelPropertyGroupItemLanguage();
            policyHotelPropertyGroupItemLanguage = policyHotelPropertyGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyHotelPropertyGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Add Linked Information
            policyHotelPropertyGroupItemLanguageRepository.EditItemForDisplay(policyHotelPropertyGroupItemLanguage);

            //Return View
            return View(policyHotelPropertyGroupItemLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PolicyHotelPropertyGroupItemLanguage policyHotelPropertyGroupItemLanguage = new PolicyHotelPropertyGroupItemLanguage();
            policyHotelPropertyGroupItemLanguage = policyHotelPropertyGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyHotelPropertyGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyHotelPropertyGroupItem policyHotelPropertyGroupItem = new PolicyHotelPropertyGroupItem();
            policyHotelPropertyGroupItem = policyHotelPropertyGroupItemRepository.GetPolicyHotelPropertyGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyHotelPropertyGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policyHotelPropertyGroupItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyHotelPropertyGroupItemLanguageRepository.Delete(policyHotelPropertyGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/HotelPropertyAdvice.mvc/Delete/" + policyHotelPropertyGroupItemLanguage.PolicyHotelPropertyGroupItemId.ToString() + "/" + policyHotelPropertyGroupItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            //Return
            return RedirectToAction("List", new { id = policyHotelPropertyGroupItemLanguage.PolicyHotelPropertyGroupItemId });
        }
    }
}
