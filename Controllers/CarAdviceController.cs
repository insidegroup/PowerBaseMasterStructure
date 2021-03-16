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
    public class CarAdviceController : Controller
    {
        //main repositories
        PolicyCarVendorGroupItemLanguageRepository policyCarVendorGroupItemLanguageRepository = new PolicyCarVendorGroupItemLanguageRepository();
        PolicyCarVendorGroupItemRepository policyCarVendorGroupItemRepository = new PolicyCarVendorGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyCarVendorGroupItem
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(id);

            //Check Exists
            if (policyCarVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyCarVendorGroupItemID"] = policyCarVendorGroupItem.PolicyCarVendorGroupItemId;
            ViewData["PolicyGroupID"] = policyCarVendorGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyCarVendorGroupItem.PolicyGroupId).PolicyGroupName;


            //SortField+SortOrder settings
            if (sortField !="CarAdvice")
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
            var cwtPaginatedList = policyCarVendorGroupItemLanguageRepository.PagePolicyCarVendorGroupItemCarAdvice(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage = new PolicyCarVendorGroupItemLanguage();
            policyCarVendorGroupItemLanguage = policyCarVendorGroupItemLanguageRepository.GetItem(id, languageCode);
            if (policyCarVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            policyCarVendorGroupItemLanguageRepository.EditItemForDisplay(policyCarVendorGroupItemLanguage);
            return View(policyCarVendorGroupItemLanguage);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyCarVendorGroupItem
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(id);

            //Check Exists
            if (policyCarVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //New PolicyCarVendorGroupItemLanguage
            PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage = new PolicyCarVendorGroupItemLanguage();
            policyCarVendorGroupItemLanguage.PolicyCarVendorGroupItemId = id;
            policyCarVendorGroupItemLanguageRepository.EditItemForDisplay(policyCarVendorGroupItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(policyCarVendorGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(policyCarVendorGroupItemLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage)
        {
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(policyCarVendorGroupItemLanguage.PolicyCarVendorGroupItemId);

            //Check Exists
            if (policyCarVendorGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(policyCarVendorGroupItemLanguage);
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
                policyCarVendorGroupItemLanguageRepository.Add(policyCarVendorGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");    
            }

            return RedirectToAction("List", new { id = policyCarVendorGroupItem.PolicyCarVendorGroupItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage = new PolicyCarVendorGroupItemLanguage();
            policyCarVendorGroupItemLanguage = policyCarVendorGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCarVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(policyCarVendorGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            policyCarVendorGroupItemLanguageRepository.EditItemForDisplay(policyCarVendorGroupItemLanguage);
            return View(policyCarVendorGroupItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int policyCarVendorGroupItemId, string languageCode, string carAdvice)
        {
            //Get Item 
            PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage = new PolicyCarVendorGroupItemLanguage();
            policyCarVendorGroupItemLanguage = policyCarVendorGroupItemLanguageRepository.GetItem(policyCarVendorGroupItemId, languageCode);

            //Check Exists
            if (policyCarVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(policyCarVendorGroupItemId);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyCarVendorGroupItemLanguage);
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
                policyCarVendorGroupItemLanguageRepository.Update(policyCarVendorGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CarAdvice.mvc/Edit/" + policyCarVendorGroupItemLanguage.PolicyCarVendorGroupItemId.ToString() + "/" + languageCode;
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            return RedirectToAction("List", new { id = policyCarVendorGroupItemLanguage.PolicyCarVendorGroupItemId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage = new PolicyCarVendorGroupItemLanguage();
            policyCarVendorGroupItemLanguage = policyCarVendorGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCarVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Add Linked Information
            policyCarVendorGroupItemLanguageRepository.EditItemForDisplay(policyCarVendorGroupItemLanguage);

            //Return View
            return View(policyCarVendorGroupItemLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PolicyCarVendorGroupItemLanguage policyCarVendorGroupItemLanguage = new PolicyCarVendorGroupItemLanguage();
            policyCarVendorGroupItemLanguage = policyCarVendorGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCarVendorGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCarVendorGroupItem policyCarVendorGroupItem = new PolicyCarVendorGroupItem();
            policyCarVendorGroupItem = policyCarVendorGroupItemRepository.GetPolicyCarVendorGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarVendorGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policyCarVendorGroupItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyCarVendorGroupItemLanguageRepository.Delete(policyCarVendorGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CarAdvice.mvc/Delete/" + policyCarVendorGroupItem.PolicyCarVendorGroupItemId.ToString() + "/" + policyCarVendorGroupItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }


            //Return
            return RedirectToAction("List", new { id = policyCarVendorGroupItemLanguage.PolicyCarVendorGroupItemId });
        }
    }
}
