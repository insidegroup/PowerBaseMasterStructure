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
    public class MissedSavingsAdviceController : Controller
    {
        //main repositories
        PolicyAirMissedSavingsThresholdGroupItemLanguageRepository policyAirMissedSavingsThresholdGroupItemLanguageRepository = new PolicyAirMissedSavingsThresholdGroupItemLanguageRepository();
        PolicyAirMissedSavingsThresholdGroupItemRepository policyAirMissedSavingsThresholdGroupItemRepository = new PolicyAirMissedSavingsThresholdGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyAirMissedSavingsThresholdGroupItem
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(id);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyAirMissedSavingsThresholdGroupItemId"] = policyAirMissedSavingsThresholdGroupItem.PolicyAirMissedSavingsThresholdGroupItemId;
            ViewData["PolicyGroupID"] = policyAirMissedSavingsThresholdGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId).PolicyGroupName;


            //SortField+SortOrder settings
            if (sortField != "MissedSavingsAdvice")
            {
                sortField = "LanguageName";
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
                sortOrder = 0;
            }

            //Get data
            var cwtPaginatedList = policyAirMissedSavingsThresholdGroupItemLanguageRepository.PagePolicyAirMissedSavingsThresholdGroupItemMissedSavingsAdvice(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyAirMissedSavingsThresholdGroupItem
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(id);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //New PolicyAirMissedSavingsThresholdGroupItemLanguage
            PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage = new PolicyAirMissedSavingsThresholdGroupItemLanguage();
            policyAirMissedSavingsThresholdGroupItemLanguage.PolicyAirMissedSavingsThresholdGroupItemId = id;
            policyAirMissedSavingsThresholdGroupItemLanguageRepository.EditItemForDisplay(policyAirMissedSavingsThresholdGroupItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(policyAirMissedSavingsThresholdGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(policyAirMissedSavingsThresholdGroupItemLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage)
        {
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(policyAirMissedSavingsThresholdGroupItemLanguage.PolicyAirMissedSavingsThresholdGroupItemId);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                //AccessRights Error
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(policyAirMissedSavingsThresholdGroupItemLanguage);
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
                policyAirMissedSavingsThresholdGroupItemLanguageRepository.Add(policyAirMissedSavingsThresholdGroupItemLanguage);
            }
            catch
            {
                //Insert Error
                return View("Error");
            }


            return RedirectToAction("List", new { id = policyAirMissedSavingsThresholdGroupItem.PolicyAirMissedSavingsThresholdGroupItemId });
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage = new PolicyAirMissedSavingsThresholdGroupItemLanguage();
            policyAirMissedSavingsThresholdGroupItemLanguage = policyAirMissedSavingsThresholdGroupItemLanguageRepository.GetItem(id, languageCode);
            if (policyAirMissedSavingsThresholdGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            policyAirMissedSavingsThresholdGroupItemLanguageRepository.EditItemForDisplay(policyAirMissedSavingsThresholdGroupItemLanguage);
            return View(policyAirMissedSavingsThresholdGroupItemLanguage);
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage = new PolicyAirMissedSavingsThresholdGroupItemLanguage();
            policyAirMissedSavingsThresholdGroupItemLanguage = policyAirMissedSavingsThresholdGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(policyAirMissedSavingsThresholdGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            policyAirMissedSavingsThresholdGroupItemLanguageRepository.EditItemForDisplay(policyAirMissedSavingsThresholdGroupItemLanguage);
            return View(policyAirMissedSavingsThresholdGroupItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage = new PolicyAirMissedSavingsThresholdGroupItemLanguage();
            policyAirMissedSavingsThresholdGroupItemLanguage = policyAirMissedSavingsThresholdGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyAirMissedSavingsThresholdGroupItemLanguage);
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

            //Update MissedSavingsAdvice
            try
            {
                policyAirMissedSavingsThresholdGroupItemLanguageRepository.Update(policyAirMissedSavingsThresholdGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/MissedSavingsAdvice.mvc/Edit/" + policyAirMissedSavingsThresholdGroupItemLanguage.PolicyAirMissedSavingsThresholdGroupItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyAirMissedSavingsThresholdGroupItemLanguage.PolicyAirMissedSavingsThresholdGroupItemId });
        }


        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage = new PolicyAirMissedSavingsThresholdGroupItemLanguage();
            policyAirMissedSavingsThresholdGroupItemLanguage = policyAirMissedSavingsThresholdGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Add Linked Information
            policyAirMissedSavingsThresholdGroupItemLanguageRepository.EditItemForDisplay(policyAirMissedSavingsThresholdGroupItemLanguage);

            //Return View
            return View(policyAirMissedSavingsThresholdGroupItemLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PolicyAirMissedSavingsThresholdGroupItemLanguage policyAirMissedSavingsThresholdGroupItemLanguage = new PolicyAirMissedSavingsThresholdGroupItemLanguage();
            policyAirMissedSavingsThresholdGroupItemLanguage = policyAirMissedSavingsThresholdGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyAirMissedSavingsThresholdGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirMissedSavingsThresholdGroupItem policyAirMissedSavingsThresholdGroupItem = new PolicyAirMissedSavingsThresholdGroupItem();
            policyAirMissedSavingsThresholdGroupItem = policyAirMissedSavingsThresholdGroupItemRepository.GetPolicyAirMissedSavingsThresholdGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirMissedSavingsThresholdGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Delete Item
            try
            {
                policyAirMissedSavingsThresholdGroupItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyAirMissedSavingsThresholdGroupItemLanguageRepository.Delete(policyAirMissedSavingsThresholdGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/MissedSavingsAdvice.mvc/Delete/" + policyAirMissedSavingsThresholdGroupItemLanguage.PolicyAirMissedSavingsThresholdGroupItemId.ToString() + "/" + policyAirMissedSavingsThresholdGroupItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            //Return
            return RedirectToAction("List", new { id = policyAirMissedSavingsThresholdGroupItemLanguage.PolicyAirMissedSavingsThresholdGroupItemId });
        }
    }
}
