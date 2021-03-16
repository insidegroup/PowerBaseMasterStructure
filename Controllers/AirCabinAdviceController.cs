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
    public class AirCabinAdviceController : Controller
    {
        //main repositories
        PolicyAirCabinGroupItemLanguageRepository policyAirCabinGroupItemLanguageRepository = new PolicyAirCabinGroupItemLanguageRepository();
        PolicyAirCabinGroupItemRepository policyAirCabinGroupItemRepository = new PolicyAirCabinGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyAirCabinGroupItem
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);

            //Check Exists
            if (policyAirCabinGroupItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyAirCabinGroupItemID"] = policyAirCabinGroupItem.PolicyAirCabinGroupItemId;
            ViewData["PolicyGroupID"] = policyAirCabinGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyAirCabinGroupItem.PolicyGroupId).PolicyGroupName;


            //SortField+SortOrder settings
            if (sortField != "AirCabinAdvice")
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
            var cwtPaginatedList = policyAirCabinGroupItemLanguageRepository.PagePolicyAirCabinGroupItemAirCabinAdvice(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage = new PolicyAirCabinGroupItemLanguage();
            policyAirCabinGroupItemLanguage = policyAirCabinGroupItemLanguageRepository.GetItem(id, languageCode);
            if (policyAirCabinGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            policyAirCabinGroupItemLanguageRepository.EditItemForDisplay(policyAirCabinGroupItemLanguage);
            return View(policyAirCabinGroupItemLanguage);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyAirCabinGroupItem
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);

            //Check Exists
            if (policyAirCabinGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //New PolicyAirCabinGroupItemLanguage
            PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage = new PolicyAirCabinGroupItemLanguage();
            policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId = id;
            policyAirCabinGroupItemLanguageRepository.EditItemForDisplay(policyAirCabinGroupItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(policyAirCabinGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            PolicyGroup policyGroup = new PolicyGroup();
            policyGroup = policyGroupRepository.GetGroup(policyAirCabinGroupItemLanguage.PolicyGroupId);

            ViewData["PolicyGroupName"] = policyGroup.PolicyGroupName;
            ViewData["PolicyGroupId"] = policyGroup.PolicyGroupId;
            ViewData["PolicyAirCabinGroupItemId"] = id;


            //Show Create Form
            return View(policyAirCabinGroupItemLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage)
        {
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId);
            
            //Check Exists
            if (policyAirCabinGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
                //AccessRights Error
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");

            }

            //Update  Model from Form
            try
            {
                UpdateModel(policyAirCabinGroupItemLanguage);
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
                policyAirCabinGroupItemLanguageRepository.Add(policyAirCabinGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }


            return RedirectToAction("List", new { id = policyAirCabinGroupItem.PolicyAirCabinGroupItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage = new PolicyAirCabinGroupItemLanguage();
            policyAirCabinGroupItemLanguage = policyAirCabinGroupItemLanguageRepository.GetItem(id,languageCode);

            //Check Exists
            if (policyAirCabinGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(policyAirCabinGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Parent Information
            ViewData["PolicyGroupID"] = policyAirCabinGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyAirCabinGroupItem.PolicyGroupId).PolicyGroupName;

            policyAirCabinGroupItemLanguageRepository.EditItemForDisplay(policyAirCabinGroupItemLanguage);
            return View(policyAirCabinGroupItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage = new PolicyAirCabinGroupItemLanguage();
            policyAirCabinGroupItemLanguage = policyAirCabinGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyAirCabinGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyAirCabinGroupItemLanguage);
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
                policyAirCabinGroupItemLanguageRepository.Update(policyAirCabinGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/AirlineAdvice.mvc/Edit/" + policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId.ToString();
                    return View("VersionError");
                }
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage = new PolicyAirCabinGroupItemLanguage();
            policyAirCabinGroupItemLanguage = policyAirCabinGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyAirCabinGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Parent Information
            ViewData["PolicyGroupID"] = policyAirCabinGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyAirCabinGroupItem.PolicyGroupId).PolicyGroupName;

            //Add Linked Information
            policyAirCabinGroupItemLanguageRepository.EditItemForDisplay(policyAirCabinGroupItemLanguage);

            //Return View
            return View(policyAirCabinGroupItemLanguage);

            }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PolicyAirCabinGroupItemLanguage policyAirCabinGroupItemLanguage = new PolicyAirCabinGroupItemLanguage();
            policyAirCabinGroupItemLanguage = policyAirCabinGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyAirCabinGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyAirCabinGroupItem policyAirCabinGroupItem = new PolicyAirCabinGroupItem();
            policyAirCabinGroupItem = policyAirCabinGroupItemRepository.GetPolicyAirCabinGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyAirCabinGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policyAirCabinGroupItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyAirCabinGroupItemLanguageRepository.Delete(policyAirCabinGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/AirlineAdvice.mvc/Delete/" + policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId.ToString() + "/" + policyAirCabinGroupItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }


            //Return
            return RedirectToAction("List", new { id = policyAirCabinGroupItemLanguage.PolicyAirCabinGroupItemId });
        }
    }
}
