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
    public class CarTypeAdviceController : Controller
    {
        //main repositories
        PolicyCarTypeGroupItemLanguageRepository policyCarTypeGroupItemLanguageRepository = new PolicyCarTypeGroupItemLanguageRepository();
        PolicyCarTypeGroupItemRepository policyCarTypeGroupItemRepository = new PolicyCarTypeGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyCarTypeGroupItem
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);

            //Check Exists
            if (policyCarTypeGroupItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyCarTypeGroupItemID"] = policyCarTypeGroupItem.PolicyCarTypeGroupItemId;
            ViewData["PolicyGroupID"] = policyCarTypeGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyCarTypeGroupItem.PolicyGroupId).PolicyGroupName;


            //SortField+SortOrder settings
            if (sortField != "CarTypeAdvice")
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
            var cwtPaginatedList = policyCarTypeGroupItemLanguageRepository.PagePolicyCarTypeGroupItemCarTypeAdvice(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage = new PolicyCarTypeGroupItemLanguage();
            policyCarTypeGroupItemLanguage = policyCarTypeGroupItemLanguageRepository.GetItem(id, languageCode);
            if (policyCarTypeGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            policyCarTypeGroupItemLanguageRepository.EditItemForDisplay(policyCarTypeGroupItemLanguage);
            return View(policyCarTypeGroupItemLanguage);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyCarTypeGroupItem
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);

            //Check Exists
            if (policyCarTypeGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //New PolicyCarTypeGroupItemLanguage
            PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage = new PolicyCarTypeGroupItemLanguage();
            policyCarTypeGroupItemLanguage.PolicyCarTypeGroupItemId = id;
            policyCarTypeGroupItemLanguageRepository.EditItemForDisplay(policyCarTypeGroupItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(policyCarTypeGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(policyCarTypeGroupItemLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage)
        {
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(policyCarTypeGroupItemLanguage.PolicyCarTypeGroupItemId);

            //Check Exists
            if (policyCarTypeGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                //AccessRights Error
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(policyCarTypeGroupItemLanguage);
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
                policyCarTypeGroupItemLanguageRepository.Add(policyCarTypeGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                LogRepository logRepository = new LogRepository();
                logRepository.LogError(ex.Message);

                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");
            }

            return RedirectToAction("List", new { id = policyCarTypeGroupItem.PolicyCarTypeGroupItemId });
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage = new PolicyCarTypeGroupItemLanguage();
            policyCarTypeGroupItemLanguage = policyCarTypeGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCarTypeGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(policyCarTypeGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            policyCarTypeGroupItemLanguageRepository.EditItemForDisplay(policyCarTypeGroupItemLanguage);
            return View(policyCarTypeGroupItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, string carAdvice)
        {
            //Get Item 
            PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage = new PolicyCarTypeGroupItemLanguage();
            policyCarTypeGroupItemLanguage = policyCarTypeGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCarTypeGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyCarTypeGroupItemLanguage);
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
                policyCarTypeGroupItemLanguageRepository.Update(policyCarTypeGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CarTypeAdvice.mvc/Edit/" + policyCarTypeGroupItemLanguage.PolicyCarTypeGroupItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            return RedirectToAction("List", new { id = policyCarTypeGroupItemLanguage.PolicyCarTypeGroupItemId });
        }

        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage = new PolicyCarTypeGroupItemLanguage();
            policyCarTypeGroupItemLanguage = policyCarTypeGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCarTypeGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Add Linked Information
            policyCarTypeGroupItemLanguageRepository.EditItemForDisplay(policyCarTypeGroupItemLanguage);

            //Return View
            return View(policyCarTypeGroupItemLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PolicyCarTypeGroupItemLanguage policyCarTypeGroupItemLanguage = new PolicyCarTypeGroupItemLanguage();
            policyCarTypeGroupItemLanguage = policyCarTypeGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCarTypeGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCarTypeGroupItem policyCarTypeGroupItem = new PolicyCarTypeGroupItem();
            policyCarTypeGroupItem = policyCarTypeGroupItemRepository.GetPolicyCarTypeGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCarTypeGroupItem.PolicyGroupId))
            {
                ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Delete Item
            try
            {
                policyCarTypeGroupItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyCarTypeGroupItemLanguageRepository.Delete(policyCarTypeGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CarTypeAdvice.mvc/Delete/" + policyCarTypeGroupItemLanguage.PolicyCarTypeGroupItemId.ToString() + "/" + policyCarTypeGroupItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            
            }

            //Return
            return RedirectToAction("List", new { id = policyCarTypeGroupItemLanguage.PolicyCarTypeGroupItemId });
        }
    }
}