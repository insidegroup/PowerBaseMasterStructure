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
    public class CountryAdviceController : Controller
    {
        //main repositories
        PolicyCountryGroupItemLanguageRepository policyCountryGroupItemLanguageRepository = new PolicyCountryGroupItemLanguageRepository();
        PolicyCountryGroupItemRepository policyCountryGroupItemRepository = new PolicyCountryGroupItemRepository();
        PolicyGroupRepository policyGroupRepository = new PolicyGroupRepository();

        //GET:List
        public ActionResult List(int id, int? page, string sortField, int? sortOrder)
        {
            //Get PolicyCountryGroupItem
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);

            //Check Exists
            if (policyCountryGroupItem == null)
            {
                ViewData["ActionMethod"] = "ListGet";
                return View("RecordDoesNotExistError");
            }

            //Set Access Rights
            ViewData["Access"] = "";
            RolesRepository rolesRepository = new RolesRepository();
            if (rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                ViewData["Access"] = "WriteAccess";
            }

            //Parent Information
            ViewData["PolicyCountryGroupItemId"] = policyCountryGroupItem.PolicyCountryGroupItemId;
            ViewData["PolicyGroupID"] = policyCountryGroupItem.PolicyGroupId;
            ViewData["PolicyGroupName"] = policyGroupRepository.GetGroup(policyCountryGroupItem.PolicyGroupId).PolicyGroupName;


            //SortField+SortOrder settings
            if (sortField != "CountryAdvice")
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
            var cwtPaginatedList = policyCountryGroupItemLanguageRepository.PagePolicyCountryGroupItemCountryAdvice(id, page ?? 1, sortField, sortOrder ?? 0);
            return View(cwtPaginatedList);
        }

        // GET: /Create
        public ActionResult Create(int id)
        {
            //Get PolicyCountryGroupItem
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);

            //Check Exists
            if (policyCountryGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreateGet";
                return View("RecordDoesNotExistError");
            }

            //AccessRights
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //New PolicyCountryGroupItemLanguage
            PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage = new PolicyCountryGroupItemLanguage();
            policyCountryGroupItemLanguage.PolicyCountryGroupItemId = id;
            policyCountryGroupItemLanguageRepository.EditItemForDisplay(policyCountryGroupItemLanguage);

            //Language SelectList
            SelectList languageList = new SelectList(policyCountryGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            //Show Create Form
            return View(policyCountryGroupItemLanguage);
        }

        // POST: /Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage)
        {
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(policyCountryGroupItemLanguage.PolicyCountryGroupItemId);

            //Check Exists
            if (policyCountryGroupItem == null)
            {
                ViewData["ActionMethod"] = "CreatePost";
                return View("RecordDoesNotExistError");
            }

            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                //AccessRights Error
                return View("Error");
            }

            //Update  Model from Form
            try
            {
                UpdateModel(policyCountryGroupItemLanguage);
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
                policyCountryGroupItemLanguageRepository.Add(policyCountryGroupItemLanguage);
            }
            catch
            {
                //Insert Error
                return View("Error");
            }
            

            return RedirectToAction("List", new { id = policyCountryGroupItem.PolicyCountryGroupItemId });
        }

        //GET: View
        public ActionResult View(int id, string languageCode)
        {
            PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage = new PolicyCountryGroupItemLanguage();
            policyCountryGroupItemLanguage = policyCountryGroupItemLanguageRepository.GetItem(id, languageCode);
            if (policyCountryGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "ViewGet";
                return View("RecordDoesNotExistError");
            }
            policyCountryGroupItemLanguageRepository.EditItemForDisplay(policyCountryGroupItemLanguage);
            return View(policyCountryGroupItemLanguage);
        }

        // GET: /Edit
        public ActionResult Edit(int id, string languageCode)
        {
            //Get Item 
            PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage = new PolicyCountryGroupItemLanguage();
            policyCountryGroupItemLanguage = policyCountryGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCountryGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Language SelectList
            SelectList languageList = new SelectList(policyCountryGroupItemLanguageRepository.GetUnUsedLanguages(id).ToList(), "LanguageCode", "LanguageName");
            ViewData["Languages"] = languageList;

            policyCountryGroupItemLanguageRepository.EditItemForDisplay(policyCountryGroupItemLanguage);
            return View(policyCountryGroupItemLanguage);
        }

        // POST: /Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string languageCode, FormCollection formCollection)
        {
            //Get Item 
            PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage = new PolicyCountryGroupItemLanguage();
            policyCountryGroupItemLanguage = policyCountryGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCountryGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "EditPost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
		        ViewData["Message"] = "You do not have access to this item";
                return View("Error");
            }

            //Update Item from Form
            try
            {
                UpdateModel(policyCountryGroupItemLanguage);
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
                policyCountryGroupItemLanguageRepository.Update(policyCountryGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CountryAdvice.mvc/Edit/" + policyCountryGroupItemLanguage.PolicyCountryGroupItemId.ToString();
                    return View("VersionError");
                }

                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }

            return RedirectToAction("List", new { id = policyCountryGroupItemLanguage.PolicyCountryGroupItemId });
        }


        // GET: /Delete
		[HttpGet]
		public ActionResult Delete(int id, string languageCode)
        {
            //Get Item 
            PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage = new PolicyCountryGroupItemLanguage();
            policyCountryGroupItemLanguage = policyCountryGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCountryGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeleteGet";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Add Linked Information
            policyCountryGroupItemLanguageRepository.EditItemForDisplay(policyCountryGroupItemLanguage);

            //Return View
            return View(policyCountryGroupItemLanguage);

        }

        // POST: /Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, string languageCode, FormCollection collection)
        {
            //Get Item 
            PolicyCountryGroupItemLanguage policyCountryGroupItemLanguage = new PolicyCountryGroupItemLanguage();
            policyCountryGroupItemLanguage = policyCountryGroupItemLanguageRepository.GetItem(id, languageCode);

            //Check Exists
            if (policyCountryGroupItemLanguage == null)
            {
                ViewData["ActionMethod"] = "DeletePost";
                return View("RecordDoesNotExistError");
            }

            //Check AccessRights
            PolicyCountryGroupItem policyCountryGroupItem = new PolicyCountryGroupItem();
            policyCountryGroupItem = policyCountryGroupItemRepository.GetPolicyCountryGroupItem(id);
            RolesRepository rolesRepository = new RolesRepository();
            if (!rolesRepository.HasWriteAccessToPolicyGroup(policyCountryGroupItem.PolicyGroupId))
            {
                return View("Error");
            }

            //Delete Item
            try
            {
                policyCountryGroupItemLanguage.VersionNumber = Int32.Parse(collection["VersionNumber"]);
                policyCountryGroupItemLanguageRepository.Delete(policyCountryGroupItemLanguage);
            }
            catch (SqlException ex)
            {
                //Versioning Error - go to standard versionError page
                if (ex.Message == "SQLVersioningError")
                {
                    ViewData["ReturnURL"] = "/CountryAdvice.mvc/Delete/" + policyCountryGroupItemLanguage.PolicyCountryGroupItemId.ToString() + "/" + policyCountryGroupItemLanguage.LanguageCode;
                    return View("VersionError");
                }
                //Generic Error
                ViewData["Message"] = "There was a problem with your request, please see the log file or contact an administrator for details";
                return View("Error");            }


            //Return
            return RedirectToAction("List", new { id = policyCountryGroupItemLanguage.PolicyCountryGroupItemId });
        }
    }
}
